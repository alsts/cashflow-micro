using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Common.Data.DataObjects;
using TaskService.Data.Models;
using TaskService.Data.Repos.Interfaces;
using TaskService.Services.Income.interfaces;

namespace TaskService.Services.Income
{
    public class TaskJobIncomeService : ITaskJobIncomeService
    {
        private readonly IUserRepo userRepo;
        private readonly LoggedInUserDataHolder loggedInUserDataHolder;

        public TaskJobIncomeService(IUserRepo userRepo, LoggedInUserDataHolder loggedInUserDataHolder)
        {
            this.userRepo = userRepo;
            this.loggedInUserDataHolder = loggedInUserDataHolder;
        }

        public Task<User> GetCurrent()
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetByPublicId(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}
