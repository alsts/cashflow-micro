using System;
using AutoMapper;
using Cashflow.Common.Events.Accounts;
using MassTransit;
using Microsoft.Extensions.Logging;
using MoneyService.Data.Models;
using MoneyService.Data.Repos.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace MoneyService.Events
{
    public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly IMapper mapper;
        private readonly ILogger<UserCreatedConsumer> logger;
        private readonly IUserRepo userRepo;
        
        public UserCreatedConsumer(IMapper mapper, ILogger<UserCreatedConsumer> logger, IUserRepo userRepo)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.userRepo = userRepo;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var userFromEvent = mapper.Map<User>(context.Message);

            if (userFromEvent == null)
            {
                logger.LogError($"[User Created Event] - Failed - User event is null");
                return; // remove broken event from queue
            }
            
            if (await userRepo.GetByPublicId(userFromEvent.PublicId) != null)
            {
                var errorMessage = $"[User Created Event] - Failed - User already exists [User: {userFromEvent.PublicId}]";
                logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }
            
            await userRepo.Save(userFromEvent);
            logger.LogInformation($"[User Created Event] - Processed [User: {userFromEvent.PublicId}, Version: {userFromEvent.Version}]");
        }
    }
}
