
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyService.Data.Models;

namespace MoneyService.Services.interfaces
{
    public interface IMoneyTasksService
    {
        Task<decimal> GetTaskBalance(string taskId);
        Task<TaskTransaction> AddMoneyToTaskBalance(string taskId, decimal amount);
        Task<TaskTransaction> ReturnMoneyFromTaskBalance(string taskId);
        Task<List<TaskTransaction>> GetTaskTransactionsHistoryForCurrentUser();
    }
}
