using Task = System.Threading.Tasks.Task;
using TaskEntity = TaskService.Data.Models.Task;

namespace TaskService.Events.Publishers.Interfaces
{
    public interface IMessageBusPublisher
    {
        bool IsEventBusHealthy();
        
        Task PublishCreatedTask(TaskEntity task);
        Task PublishUpdatedTask(TaskEntity task);
    }
}
