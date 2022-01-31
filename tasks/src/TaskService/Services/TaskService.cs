using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using AccountService.Util;
using TaskService.Data.Repos.Interfaces;
using TaskService.Dtos;
using TaskService.Services.interfaces;
using TaskService.Util.DataObjects;
using TaskEntity = TaskService.Data.Models.Task;

namespace TaskService.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepo taskRepo;
        private readonly IUserService userService;
        private readonly LoggedInUserDataHolder loggedInUserDataHolder;

        public TaskService(
            ITaskRepo taskRepo,
            IUserService userService,
            LoggedInUserDataHolder loggedInUserDataHolder)
        {
            this.taskRepo = taskRepo;
            this.userService = userService;
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
                CreatedAt = DateTime.Now,
                IsActive = false,
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
            return task;
        }

        public async Task<IEnumerable<TaskEntity>> GetAll()
        {
            return await taskRepo.GetAll();
        }
    }
}
