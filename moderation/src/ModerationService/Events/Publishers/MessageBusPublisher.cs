using System;
using AutoMapper;
using Cashflow.Common.Events.Moderation;
using MassTransit;
using MassTransit.Monitoring.Health;
using Microsoft.Extensions.Logging;
using ModerationService.Data.Models;
using ModerationService.Data.Models.External;
using ModerationService.Events.Publishers.Interfaces;
using Task = System.Threading.Tasks.Task;
using TaskEntity = ModerationService.Data.Models.External.Task;

namespace ModerationService.Events.Publishers
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
        
        public async Task PublishUserBannedEvent(UserBan userBan)
        {
            var userBannedEvent = mapper.Map<UserBannedEvent>(userBan);
            logger.LogInformation($"[User Banned Event] - Published [User: {userBannedEvent.UserId}]");
            await publishEndpoint.Publish(userBannedEvent);
        }

        public async Task PublishTaskApprovedEvent(TaskEntity task)
        {
            var eventMessage = mapper.Map<TaskApprovedEvent>(task);
            logger.LogInformation($"[Task Approved Event] - Published [Task: {eventMessage.TaskId}]");
            await publishEndpoint.Publish(eventMessage);
        }
    }
}
