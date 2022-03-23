using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyService.Data.Models;
using TaskEntity = MoneyService.Data.Models.External.Task;
using Task = System.Threading.Tasks.Task;

namespace MoneyService.Data.Repos.Interfaces
{
    public interface ITaskTransactionRepo
    {
        Task Save(TaskTransaction taskTransaction);
        
        Task<decimal> GetTaskBalance(int taskId);
        Task<decimal> GetTaskPendingBalance(int taskId);
        Task<List<TaskTransaction>> GetTransactionsHistory(string userId);
    }
}
