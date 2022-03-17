using Cashflow.Common.Data.DataObjects;
using MoneyService.Data.Repos.Interfaces;
using MoneyService.Services.interfaces;

namespace MoneyService.Services
{
    public class MoneyTasksTasksService : IMoneyTasksService
    {
        private readonly ITaskRepo taskRepo;
        private readonly IUserRepo userRepo;
        private readonly LoggedInUserDataHolder loggedInUserDataHolder;

        public MoneyTasksTasksService(
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
