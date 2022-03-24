using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cashflow.Common.Data.Enums;
using Microsoft.EntityFrameworkCore;
using TaskService.Data.Models.External;
using TaskService.Data.Repos.Interfaces;

namespace TaskService.Data.Repos
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

        public async Task<decimal> GetTaskBalance(string taskId)
        {
            var statuses = new List<int>
            {
                (int)TransactionStatus.Completed,
                (int)TransactionStatus.Reserved,
                (int)TransactionStatus.Pending
            };

            return await context.TaskTransactions
                .Where(x => x.TaskId == taskId && statuses.Contains(x.TransactionStatus))
                .SumAsync(x => x.Amount);
        }

        public async Task<TaskTransaction> GetByPublicId(string taskTransactionId)
        {
            return await context.TaskTransactions
                .Where(x => x.PublicId == taskTransactionId)
                .FirstAsync();
        }

        private async Task<bool> SaveChanges()
        {
            return await context.SaveChangesAsync() >= 0;
        }
    }
}
