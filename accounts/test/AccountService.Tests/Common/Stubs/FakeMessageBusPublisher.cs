using System.Threading.Tasks;
using AccountService.Data.Models;
using AccountService.Events.Publishers.Interfaces;

namespace AccountService.Tests.Common.Stubs
{
    public class FakeMessageBusPublisher : IMessageBusPublisher
    {
        public bool IsEventBusHealthy()
        {
            return true;
        }

        public Task PublishCreatedUser(User user)
        {
            return Task.CompletedTask;
        }

        public Task PublishUpdatedUser(User user)
        {
            return Task.CompletedTask;
        }
    }
}
