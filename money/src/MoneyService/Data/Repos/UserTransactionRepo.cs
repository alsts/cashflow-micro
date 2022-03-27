using System.Linq;
using System.Threading.Tasks;
using Cashflow.Common.Data.Enums;
using Microsoft.EntityFrameworkCore;
using MoneyService.Data.Models;
using MoneyService.Data.Repos.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace MoneyService.Data.Repos
{
    public class UserTransactionRepo : IUserTransactionRepo
    {
        private readonly AppDbContext context;
        
        public UserTransactionRepo(AppDbContext context)
        {
            this.context = context;
        }
        
        public async Task Save(UserTransaction userTransaction)
        {
            if (userTransaction.Id != 0)
            {
                context.UserTransactions.Update(userTransaction);
            }
            else
            {
                await context.UserTransactions.AddAsync(userTransaction);
            }
            
            await SaveChanges();
        }
        
        public async Task<decimal> GetMainBalanceForUser(int userId)
        {
            return await context.UserTransactions
                .Where(x => x.UserId == userId && x.TransactionType != (int) TransactionType.AdBalance)
                .SumAsync(x => x.Amount);
        }
        
        public async Task<decimal> GetAdBalanceForUser(int userId)
        {
            return await context.UserTransactions
                .Where(x => x.UserId == userId && x.TransactionType == (int) TransactionType.AdBalance)
                .SumAsync(x => x.Amount);
        }

        private async Task<bool> SaveChanges()
        {
            return await context.SaveChangesAsync() >= 0;
        }
    }
}
