using ModerationService.Data.Models;
using ModerationService.Data.Models.External;
using Task = System.Threading.Tasks.Task;
using TaskEntity = ModerationService.Data.Models.External.Task;

namespace ModerationService.Events.Publishers.Interfaces
{
    public interface IMessageBusPublisher
    {
        bool IsEventBusHealthy();
        Task PublishUserBannedEvent(UserBan userBan);
        Task PublishTaskApprovedEvent(TaskEntity task);
    }
}
