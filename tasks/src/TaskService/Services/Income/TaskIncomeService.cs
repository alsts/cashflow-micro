using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Common.Data.DataObjects;
using TaskService.Data.Repos.Interfaces;
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

        public Task<TaskEntity> GetByPublicId(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaskEntity>> GetAvailableTasks()
        {
            throw new NotImplementedException();
        }
    }
}
