using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Cashflow.Common.Data.DataObjects;
using Cashflow.Common.Exceptions;
using ModerationService.Data.Repos.Interfaces;
using ModerationService.Services.interfaces;
using TaskEntity = ModerationService.Data.Models.External.Task;
using TaskStatus = Cashflow.Common.Data.Enums.TaskStatus;

namespace ModerationService.Services
{
    public class TaskTaskModerationService : ITaskModerationService
    {
        private readonly ITaskRepo taskRepo;
        private readonly IUserRepo userRepo;
        private readonly LoggedInUserDataHolder loggedInUserDataHolder;

        public TaskTaskModerationService(
            ITaskRepo taskRepo,
            IUserRepo userRepo,
            LoggedInUserDataHolder loggedInUserDataHolder)
        {
            this.taskRepo = taskRepo;
            this.userRepo = userRepo;
            this.loggedInUserDataHolder = loggedInUserDataHolder;
        }

        public async Task<IEnumerable<TaskEntity>> GetTasksToModerate()
        {
            return await taskRepo.GetTasksPendingApproval();
        }

        public async Task<TaskEntity> ApproveTask(string taskId)
        {
            var task = await taskRepo.GetByPublicId(taskId);
            if (task == null)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, "Task not found");
            }

            task.TaskStatus = TaskStatus.Stopped;
            task.LastUpdatedAt = DateTime.Now;
            await taskRepo.Save(task);

            return task;
        }
    }
}
