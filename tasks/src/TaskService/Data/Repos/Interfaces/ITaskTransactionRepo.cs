using System.Threading.Tasks;
using TaskService.Data.Models.External;
using TaskEntity = TaskService.Data.Models.Task;

namespace TaskService.Data.Repos.Interfaces
{
    public interface ITaskTransactionRepo
    {
        Task Save(TaskTransaction taskTransaction);
        Task<decimal> GetTaskBalance(string taskId);
        Task<TaskTransaction> GetByPublicId(string taskTransactionId);
    }
}
