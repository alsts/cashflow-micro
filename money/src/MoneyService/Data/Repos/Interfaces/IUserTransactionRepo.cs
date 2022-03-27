using System.Threading.Tasks;
using MoneyService.Data.Models;
using Task = System.Threading.Tasks.Task;

namespace MoneyService.Data.Repos.Interfaces
{
    public interface IUserTransactionRepo
    {
        Task Save(UserTransaction userTransaction);
        Task<decimal> GetMainBalanceForUser(int userId);
        Task<decimal> GetAdBalanceForUser(int userId);
    }
}
