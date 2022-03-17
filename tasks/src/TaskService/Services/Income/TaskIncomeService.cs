using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Common.Data.DataObjects;
using TaskService.Controllers.Promotion;
using TaskService.Data.Repos.Interfaces;
using TaskService.Dtos;
using TaskService.Dtos.Promotion;
using TaskService.Services.General.interfaces;
using TaskService.Services.Income.interfaces;
using TaskEntity = TaskService.Data.Models.Task;

namespace TaskService.Services.Income
{
    public class TaskIncomeService : ITaskIncomeService
    {
        private readonly ITaskRepo taskRepo;
        private readonly IUserService userService;
        private readonly LoggedInUserDataHolder loggedInUserDataHolder;

        public TaskIncomeService(
            ITaskRepo taskRepo,
            IUserService userService,
            LoggedInUserDataHolder loggedInUserDataHolder)
        {
            this.taskRepo = taskRepo;
            this.userService = userService;
            this.loggedInUserDataHolder = loggedInUserDataHolder;
        }

        public Task<TaskEntity> Create(TaskCreateDto taskCreateDto)
        {
            throw new NotImplementedException();
        }

        public Task<TaskEntity> Update(TaskUpdateDto model, string publicId)
        {
            throw new NotImplementedException();
        }

        public Task<TaskEntity> GetByPublicId(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaskEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaskEntity>> GetForCurrentUser()
        {
            throw new NotImplementedException();
        }

        public Task StartTask(string publicId)
        {
            throw new NotImplementedException();
        }

        public Task StopTask(string publicId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaskEntity>> GetAvailableTasks()
        {
            throw new NotImplementedException();
        }

        public Task GetJobsByQuery(TasksQuery tasksQuery)
        {
            throw new NotImplementedException();
        }
    }
}
