using AutoMapper;
using Cashflow.Common.Events.Money;
using MassTransit;
using MassTransit.Monitoring.Health;
using Microsoft.Extensions.Logging;
using MoneyService.Data.Models;
using MoneyService.Events.Publishers.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace MoneyService.Events.Publishers
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
        
        public async Task PublishCreatedTaskTransaction(TaskTransaction taskTransaction)
        {
            var eventMessage = mapper.Map<TaskTransactionCreatedEvent>(taskTransaction);
            logger.LogInformation($"[Task Transaction Created Event] - Published [Task Transaction: {eventMessage.PublicId}, Version: {eventMessage.Version}]");
            await publishEndpoint.Publish(eventMessage);
        }
        
        public async Task PublishCreatedTaskJobTransaction(TaskJobTransaction taskJobTransaction)
        {
            var eventMessage = mapper.Map<TaskJobTransactionCreatedEvent>(taskJobTransaction);
            logger.LogInformation($"[TaskJob Transaction Created Event] - Published [TaskJob Transaction: {eventMessage.PublicId}, Version: {eventMessage.Version}]");
            await publishEndpoint.Publish(eventMessage);
        }
        
        public async Task PublishCreatedUserTransaction(UserTransaction userTransaction)
        {
            var eventMessage = mapper.Map<UserTransactionCreatedEvent>(userTransaction);
            logger.LogInformation($"[User Transaction Created Event] - Published [User Transaction: {eventMessage.PublicId}, Version: {eventMessage.Version}]");
            await publishEndpoint.Publish(eventMessage);
        }
    }
}
