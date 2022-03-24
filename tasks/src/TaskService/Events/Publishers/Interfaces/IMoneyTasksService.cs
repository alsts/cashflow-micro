using System.Threading.Tasks;

namespace TaskService.Events.Publishers.Interfaces
{
    public interface IMoneyTasksService
    {
        Task<decimal> GetTaskBalance(string taskId);
    }
}
