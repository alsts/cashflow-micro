using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AccountService.Data.Models;
using AccountService.Dtos;
using AutoMapper;
using Cashflow.Common.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace AccountService.Events.Publishers
{
    public class MessageBusPublisher : IMessageBusPublisher
    {
        private readonly ILogger<MessageBusPublisher> logger;
        private readonly IMapper mapper;
        private readonly ISendEndpointProvider sendEndpointProvider;

        public MessageBusPublisher(
            ISendEndpointProvider sendEndpointProvider,
            ILogger<MessageBusPublisher> logger,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.sendEndpointProvider = sendEndpointProvider;
            this.logger = logger;
        }
        
        public async Task PublishCreatedUser(User user)
        {
            var eventMessage = mapper.Map<UserCreatedEvent>(user);
            var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{Queue.UserCreated}"));
            
            logger.LogInformation("Sending User Created Event: " + user.Id + ",  version: " + user.Version);
            await endpoint.Send(eventMessage);
        }

        public async Task PublishUpdatedUser(User user)
        {
            var eventMessage = mapper.Map<UserUpdatedEvent>(user);
            var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{Queue.UserUpdated}"));
            
            logger.LogInformation("Sending User Updated Event: " + user.Id + ",  version: " + user.Version);
            await endpoint.Send(eventMessage);
        }
    }
}
