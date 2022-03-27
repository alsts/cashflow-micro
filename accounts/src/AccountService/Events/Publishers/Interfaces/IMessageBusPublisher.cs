using System.Threading.Tasks;
using AccountService.Data.Models;

namespace AccountService.Events.Publishers.Interfaces
{
    public interface IMessageBusPublisher
    {
        bool IsEventBusHealthy();
        Task PublishCreatedUser(User user);
        Task PublishUpdatedUser(User user);
    }
}
