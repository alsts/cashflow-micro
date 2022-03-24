using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Cashflow.Common.Data.DataObjects;
using Cashflow.Common.Exceptions;
using TaskService.Data.Repos.Interfaces;
using TaskService.Dtos.Promotion;
using TaskService.Events.Publishers.Interfaces;
using TaskService.Services.General.interfaces;
using TaskService.Services.Promotion.interfaces;
using TaskEntity = TaskService.Data.Models.Task;
using TaskStatus = Cashflow.Common.Data.Enums.TaskStatus;

namespace TaskService.Services.Promotion
{
    public class TaskPromotionService : ITaskPromotionService
    
    {
        private readonly ITaskRepo taskRepo;
        private readonly IUserService userService;
        private readonly IMoneyTasksService moneyTasksService;
        private readonly LoggedInUserDataHolder loggedInUserDataHolder;

        public TaskPromotionService(
            ITaskRepo taskRepo,
            IUserService userService,
            IMoneyTasksService moneyTasksService,
            LoggedInUserDataHolder loggedInUserDataHolder)
        {
            this.taskRepo = taskRepo;
            this.userService = userService;
            this.moneyTasksService = moneyTasksService;
            this.loggedInUserDataHolder = loggedInUserDataHolder;
        }

        public async Task<TaskEntity> Create(TaskCreateDto taskCreateDto)
        {
            if (taskCreateDto.Title == null || taskCreateDto.Description == null)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Please fill all required fields");
            }
            
            var user = await userService.GetCurrent();

            var task = new TaskEntity
            {
                Title = taskCreateDto.Title,
                Description = taskCreateDto.Description,
                PublicId = Guid.NewGuid().ToString(),
                TaskStatus = TaskStatus.PendingApproval,
                CreatedAt = DateTime.Now,
                RewardPrice = taskCreateDto.RewardPrice,
                UserId = user.Id
            };
            
            await taskRepo.Save(task);

            task = await taskRepo.GetByPublicId(task.PublicId);
            if (task == null)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Task not created");
            }

            return task;
        }

        public async Task<TaskEntity> Update(TaskUpdateDto taskUpdateDto, string publicId)
        {
            if (String.IsNullOrEmpty(taskUpdateDto.Title) ||
                String.IsNullOrEmpty(taskUpdateDto.Description))
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Please fill all required fields");
            }

            var task = await taskRepo.GetByPublicId(publicId);
            if (task == null)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, "Task not found");
            }

            var user = await userService.GetCurrent();
            if (task.UserId != user.Id)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Not your task");
            }

            task.Title = taskUpdateDto.Title;
            task.Description = taskUpdateDto.Description;
            task.RewardPrice = taskUpdateDto.RewardPrice;

            await taskRepo.Save(task);
            return task;
        }

        public async Task<TaskEntity> GetByPublicId(string id)
        {
            var task = await taskRepo.GetByPublicId(id);
            if (task == null)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, "Task not found");
            }
            
            var user = await userService.GetCurrent();
            if (task.UserId != user.Id)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Not your task");
            }
            
            return task;
        }

        public async Task<IEnumerable<TaskEntity>> GetAll()
        {
            return await taskRepo.GetAll();
        }

        public async Task<IEnumerable<TaskEntity>> GetForCurrentUser()
        {
            var user = await userService.GetCurrent();
            return await taskRepo.GetByUserId(user.Id);
        }

        public async Task StartTask(string publicId)
        {
            var task = await taskRepo.GetByPublicId(publicId);
            if (task == null)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, "Task not found");
            }

            var user = await userService.GetCurrent();
            if (task.UserId != user.Id)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Not your task");
            }

            if (task.TaskStatus != TaskStatus.Stopped)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Can not start the task");
            }
            
            var taskAvailableBalance = await moneyTasksService.GetTaskBalance(task.PublicId);
            if (taskAvailableBalance < task.RewardPrice)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Not enough money on task balance");
            }

            task.TaskStatus = TaskStatus.Running;
            await taskRepo.Save(task);
        }

        public async Task StopTask(string publicId)
        {
            var task = await taskRepo.GetByPublicId(publicId);
            if (task == null)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, "Task not found");
            }

            var user = await userService.GetCurrent();
            if (task.UserId != user.Id)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Not your task");
            }

            if (task.TaskStatus != TaskStatus.Running)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Can not stopped the task");
            }

            task.TaskStatus = TaskStatus.Stopped;
            await taskRepo.Save(task);
        }
    }
}
