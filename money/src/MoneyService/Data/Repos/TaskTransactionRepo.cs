using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Common.Data.Enums;
using Microsoft.EntityFrameworkCore;
using MoneyService.Data.Models;
using MoneyService.Data.Repos.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace MoneyService.Data.Repos
{
    public class TaskTransactionRepo : ITaskTransactionRepo
    {
        private readonly AppDbContext context;

        public TaskTransactionRepo(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Save(TaskTransaction taskTransaction)
        {
            if (taskTransaction.Id != 0)
            {
                context.TaskTransactions.Update(taskTransaction);
            }
            else
            {
                await context.TaskTransactions.AddAsync(taskTransaction);
            }

            await SaveChanges();
        }

        public async Task<decimal> GetTaskBalance(int taskId)
        {
            var statuses = new List<int>()
            {
                (int)TransactionStatus.Completed,
                (int)TransactionStatus.Reserved,
                (int)TransactionStatus.Pending
            };

            return await context.TaskTransactions
                .Where(x => x.Id == taskId && statuses.Contains(x.TransactionStatus))
                .SumAsync(x => x.Amount);
        }

        public async Task<decimal> GetTaskPendingBalance(int taskId)
        {
            var statuses = new List<int>()
            {
                (int)TransactionStatus.Reserved,
                (int)TransactionStatus.Pending
            };
            
            // do not include pending transactions
            return await context.TaskTransactions
                .Where(x => x.Id == taskId && statuses.Contains(x.TransactionStatus))
                .SumAsync(x => x.Amount);
        }

        public async Task<List<TaskTransaction>> GetTransactionsHistory(string userId)
        {
            var statuses = new List<int>()
            {
                (int)TransactionStatus.Completed,
                (int)TransactionStatus.Reserved,
                (int)TransactionStatus.Pending
            };
            
            return await context.TaskTransactions
                .Where(x => x.CreatedByUserId == userId && statuses.Contains(x.TransactionStatus))
                .ToListAsync();
        }

        private async Task<bool> SaveChanges()
        {
            return await context.SaveChangesAsync() >= 0;
        }
    }
}
