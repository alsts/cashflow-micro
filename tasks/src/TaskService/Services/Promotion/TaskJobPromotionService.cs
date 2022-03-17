using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Common.Data.DataObjects;
using TaskService.Controllers.Promotion;
using TaskService.Data.Models;
using TaskService.Data.Repos.Interfaces;
using TaskService.Services.Promotion.interfaces;
using Task = System.Threading.Tasks.Task;

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

        public Task<TaskJob> GetById(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<TaskJob>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<TaskJob>> GetJobsByQuery(TasksQuery tasksQuery)
        {
            throw new System.NotImplementedException();
        }

        public Task Approve(string jobId)
        {
            throw new System.NotImplementedException();
        }

        public Task Decline(string jobId)
        {
            throw new System.NotImplementedException();
        }

        public Task RequestImprovement(string jobId)
        {
            throw new System.NotImplementedException();
        }
    }
}
