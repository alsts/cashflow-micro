using ModerationService.Data.Models;
using Task = System.Threading.Tasks.Task;
using TaskEntity = ModerationService.Data.Models.Task;

namespace ModerationService.Events.Publishers.Interfaces
{
    public interface IMessageBusPublisher
    {
        bool IsEventBusHealthy();
        Task PublishUserBlockedEvent(User user);
        Task PublishTaskApprovedEvent(TaskEntity task);
    }
}
