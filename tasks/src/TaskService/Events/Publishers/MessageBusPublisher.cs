using System.Threading.Tasks;
using AutoMapper;
using Cashflow.Common.Events.Tasks;
using MassTransit;
using MassTransit.Monitoring.Health;
using Microsoft.Extensions.Logging;
using TaskService.Events.Publishers.Interfaces;
using TaskEntity = TaskService.Data.Models.Task;

namespace TaskService.Events.Publishers
{
    public class MessageBusPublisher : IMessageBusPublisher
    {
        private readonly ILogger<MessageBusPublisher> logger;
        private readonly IMapper mapper;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IBusHealth busHealth;

        public MessageBusPublisher(
            IPublishEndpoint publishEndpoint,
            IBusHealth busHealth,
            ILogger<MessageBusPublisher> logger,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.publishEndpoint = publishEndpoint;
            this.logger = logger;
            this.busHealth = busHealth;
        }
        
        public bool IsEventBusHealthy()
        {
            var endpointHealthResult = busHealth.CheckHealth();
            return endpointHealthResult.Status == BusHealthStatus.Healthy;
        }

        public async Task PublishCreatedTask(TaskEntity task)
        {
            var eventMessage = mapper.Map<TaskCreatedEvent>(task);
            logger.LogInformation($"[Task Created Event] - Published [Task: {eventMessage.PublicId}, Version: {eventMessage.Version}]");
            await publishEndpoint.Publish(eventMessage);
        }

        public async Task PublishUpdatedTask(TaskEntity task)
        {
            var eventMessage = mapper.Map<TaskUpdatedEvent>(task);
            logger.LogInformation($"[Task Updated Event] - Published [Task: {eventMessage.PublicId}, Version: {eventMessage.Version}]");
            await publishEndpoint.Publish(eventMessage);
        }
    }
}
