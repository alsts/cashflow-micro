using System.Threading.Tasks;
using MoneyService.Data.Models;

namespace MoneyService.Services.interfaces
{
    public interface IMoneyUsersService
    {
        Task<decimal> GetMainBalanceForUser();
        Task<decimal> GetAdBalanceForCurrentUser();
        Task<UserTransaction> DepositToUserMainBalance(decimal amount);
        Task<(UserTransaction userMainBalanceTransaction, UserTransaction userAdBalanceTransaction)> AddMoneyToAdBalance(decimal amount);
        Task<UserTransaction> WithdrawFromMainBalance(decimal amount);
    }
}
