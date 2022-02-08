using AutoMapper;
using Cashflow.Common.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using TaskService.Data.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Events.Consumers
{
    public class UserUpdatedConsumer : IConsumer<UserUpdatedEvent>
    {
        private readonly IMapper mapper;
        private readonly ILogger<UserUpdatedConsumer> logger;

        public UserUpdatedConsumer(IMapper mapper, ILogger<UserUpdatedConsumer> logger)
        {
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
        {
            var user = mapper.Map<User>(context.Message);
            logger.LogInformation("User Updated Event: " + user.Id + ", version: " + user.Version);
        }
    }
}
