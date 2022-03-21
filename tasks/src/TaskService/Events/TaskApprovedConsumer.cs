using System;
using AutoMapper;
using Cashflow.Common.Events.Accounts;
using Cashflow.Common.Events.Moderation;
using MassTransit;
using Microsoft.Extensions.Logging;
using TaskService.Data.Models;
using TaskService.Data.Models.External;
using TaskService.Data.Repos.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Events
{
    public class TaskApprovedConsumer : IConsumer<TaskApprovedEvent>
    {
        private readonly IMapper mapper;
        private readonly ILogger<TaskApprovedEvent> logger;
        private readonly IUserRepo userRepo;
        
        public TaskApprovedConsumer(IMapper mapper, ILogger<TaskApprovedEvent> logger, IUserRepo userRepo)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.userRepo = userRepo;
        }

        public async Task Consume(ConsumeContext<TaskApprovedEvent> context)
        {
            // TODO: update!!!
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
