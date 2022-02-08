using AutoMapper;
using Cashflow.Common.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using TaskService.Data.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Events.Consumers
{
    public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly IMapper mapper;
        private readonly ILogger<UserCreatedConsumer> logger;
        
        public UserCreatedConsumer(IMapper mapper, ILogger<UserCreatedConsumer> logger)
        {
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var user = mapper.Map<User>(context.Message);
            logger.LogInformation("User Created Event: " + user.Id + ", version: " + user.Version);
        }
    }
}
