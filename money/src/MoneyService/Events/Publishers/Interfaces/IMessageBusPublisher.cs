using MoneyService.Data.Models;
using Task = System.Threading.Tasks.Task;

namespace MoneyService.Events.Publishers.Interfaces
{
    public interface IMessageBusPublisher
    {
        bool IsEventBusHealthy();
        Task PublishCreatedTaskTransaction(TaskTransaction taskTransaction);
        Task PublishCreatedUserTransaction(UserTransaction userTransaction);
    }
}
