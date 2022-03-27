using System.Threading.Tasks;
using TaskService.Data.Repos.Interfaces;
using TaskService.Events.Publishers.Interfaces;

namespace TaskService.Events.Publishers
{
    public class MoneyTasksService : IMoneyTasksService
    {
        private readonly ITaskTransactionRepo taskTransactionRepo;
        
        public MoneyTasksService(ITaskTransactionRepo taskTransactionRepo)
        {
            this.taskTransactionRepo = taskTransactionRepo;
        }
        
        public async Task<decimal> GetTaskBalance(string taskId)
        {
            return await taskTransactionRepo.GetTaskBalance(taskId);
        }
    }
}
