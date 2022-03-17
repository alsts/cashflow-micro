using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Common.Data.DataObjects;
using TaskService.Data.Models;
using TaskService.Data.Repos.Interfaces;
using TaskService.Services.Promotion.interfaces;

namespace TaskService.Services.Promotion
{
    public class TaskJobPromotionService : ITaskJobPromotionService
    {
        private readonly IUserRepo userRepo;
        private readonly LoggedInUserDataHolder loggedInUserDataHolder;

        public TaskJobPromotionService(IUserRepo userRepo, LoggedInUserDataHolder loggedInUserDataHolder)
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
