using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Cashflow.Common.Data.DataObjects;
using Cashflow.Common.Exceptions;
using TaskService.Data.Repos.Interfaces;
using TaskService.Events.Publishers.Interfaces;
using TaskService.Services.General.interfaces;
using TaskService.Services.Income.interfaces;
using TaskEntity = TaskService.Data.Models.Task;
using TaskStatus = Cashflow.Common.Data.Enums.TaskStatus;

namespace TaskService.Services.Income
{
    public class TaskIncomeService : ITaskIncomeService
    {
        private readonly ITaskRepo taskRepo;
        private readonly IUserService userService;
        private readonly LoggedInUserDataHolder loggedInUserDataHolder;
        private readonly IMoneyTasksService moneyTasksService;

        public TaskIncomeService(
            ITaskRepo taskRepo,
            IUserService userService,
            IMoneyTasksService moneyTasksService,
            LoggedInUserDataHolder loggedInUserDataHolder)
        {
            this.taskRepo = taskRepo;
            this.userService = userService;
            this.loggedInUserDataHolder = loggedInUserDataHolder;
            this.moneyTasksService = moneyTasksService;
        }

        public async Task<TaskEntity> GetByPublicId(string taskId)
        {
            var task = await taskRepo.GetByPublicId(taskId);
            if (task == null)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Task not found");
            }

            if (task.TaskStatus != TaskStatus.Running)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Task is not running");
            }
            
            task.AvailableBalance = await moneyTasksService.GetTaskBalance(task.PublicId);
            return task;
        }

        public async Task<IEnumerable<TaskEntity>> GetAvailableTasks()
        {
            var runningTasks = await taskRepo.GetTasksByStatus(TaskStatus.Running);

            foreach (var task in runningTasks)
            {
                task.AvailableBalance = await moneyTasksService.GetTaskBalance(task.PublicId);
            }

            return runningTasks;
        }
    }
}
