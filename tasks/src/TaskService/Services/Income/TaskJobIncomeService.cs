using System.Collections.Generic;
using System.Threading.Tasks;
using Cashflow.Common.Data.DataObjects;
using TaskService.Data.Models;
using TaskService.Data.Repos.Interfaces;
using TaskService.Dtos.Income;
using TaskService.Services.Income.interfaces;
using Task = System.Threading.Tasks.Task;

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

        public Task<IEnumerable<TaskJob>> GetForCurrentUser()
        {
            throw new System.NotImplementedException();
        }

        public Task<TaskJob> GetById(string jobId)
        {
            throw new System.NotImplementedException();
        }

        public Task SubmitReport(string jobId, TaskJobReportDto report)
        {
            throw new System.NotImplementedException();
        }

        public Task SubmitReportImprovement(string jobId, TaskJobReportImprovementDto reportImprovement)
        {
            throw new System.NotImplementedException();
        }

        public Task Cancel(string jobId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> CanUserStartWorkingOnTask(string taskId)
        {
            throw new System.NotImplementedException();
        }

        public Task<object> StartWorkingOnTask(string taskId)
        {
            throw new System.NotImplementedException();
        }
    }
}
