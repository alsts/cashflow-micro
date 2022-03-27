using System.Threading.Tasks;
using TaskService.Events.Publishers.Interfaces;

namespace TaskService.Tests.Common.Stubs
{
    public class FakeMessageBusPublisher : IMessageBusPublisher
    {
        public bool IsEventBusHealthy()
        {
            return true;
        }

        public Task PublishCreatedTask(Data.Models.Task task)
        {
            return Task.CompletedTask;
        }

        public Task PublishUpdatedTask(Data.Models.Task task)
        {
            return Task.CompletedTask;
        }
    }
}
