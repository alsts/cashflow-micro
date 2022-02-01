using AccountService.Dtos;

namespace AccountService.EventBus.Publisher
{
    public interface IMessageBusPublisher
    {
        void PublishNewUser(UserPublishedDto userPublishedDto);
    }
}
