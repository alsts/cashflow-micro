using PlatformService.Dtos;

namespace PlatformService.SyncData
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
    }
}
