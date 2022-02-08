using System.Threading.Tasks;
using AccountService.Data.Models;

namespace AccountService.Events.Publishers
{
    public interface IMessageBusPublisher
    {
        Task PublishCreatedUser(User user);
        Task PublishUpdatedUser(User user);
    }
}
