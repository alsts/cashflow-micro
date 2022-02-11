using System.Collections.Generic;
using System.Threading.Tasks;
using AccountService.Data.Models;
using AutoMapper;
using Cashflow.Common.Events;
using MassTransit;
using MassTransit.Monitoring.Health;
using Microsoft.Extensions.Logging;

namespace AccountService.Events
{
    public class MessageBusPublisher
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
        
        public async Task PublishCreatedUser(User user)
        {
            var eventMessage = mapper.Map<UserCreatedEvent>(user);
            logger.LogInformation($"[User Created Event] - Published [User: {eventMessage.PublicId}, Version: {eventMessage.Version}]");
            await publishEndpoint.Publish(eventMessage);
        }

        public async Task PublishUpdatedUser(User user)
        {
            var eventMessage = mapper.Map<UserUpdatedEvent>(user);
            logger.LogInformation($"[User Updated Event] - Published [User: {eventMessage.PublicId}, Version: {eventMessage.Version}]");
            await publishEndpoint.Publish(eventMessage);
        }
    }
}
