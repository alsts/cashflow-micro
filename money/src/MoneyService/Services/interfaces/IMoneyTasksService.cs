
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyService.Data.Models;
using Task = System.Threading.Tasks.Task;

namespace MoneyService.Services.interfaces
{
    public interface IMoneyTasksService
    {
        Task<decimal> GetTaskBalance(string taskId);
        Task<(UserTransaction userTransaction, TaskTransaction taskTransaction)> AddMoneyToTaskBalance(string taskId, decimal amount);
        Task<(UserTransaction userTransaction, TaskTransaction taskTransaction)> ReturnMoneyFromTaskBalance(string taskId);
        Task<List<TaskTransaction>> GetTaskTransactionsHistoryForCurrentUser();
    }
}
