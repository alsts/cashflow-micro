namespace TaskService.EventBus.Subscriber.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
