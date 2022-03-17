using Cashflow.Common.Data.DataObjects;
using ModerationService.Data.Repos.Interfaces;
using ModerationService.Services.interfaces;
using TaskEntity = ModerationService.Data.Models.Task;

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
    }
}
