using System;
using AutoMapper;
using Cashflow.Common.Events.Accounts;
using MassTransit;
using Microsoft.Extensions.Logging;
using MoneyService.Data.Models;
using MoneyService.Data.Models.External;
using MoneyService.Data.Repos.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace MoneyService.Events.Consumers
{
    public class UserUpdatedConsumer : IConsumer<UserUpdatedEvent>
    {
        private readonly IMapper mapper;
        private readonly ILogger<UserUpdatedConsumer> logger;
        private readonly IUserRepo userRepo;
        public UserUpdatedConsumer(IMapper mapper, ILogger<UserUpdatedConsumer> logger, IUserRepo userRepo)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.userRepo = userRepo;
        }

        public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
        {
            var userFromEvent = mapper.Map<User>(context.Message);
            
            if (userFromEvent == null)
            {
                logger.LogError($"[User Updated Event] - Failed - User event is null");
                return; // remove broken event from queue
            }

            var existingUser = await userRepo.GetByPublicId(userFromEvent.PublicId);
            if (existingUser == null)
            {
                // schedule redelivery (user maybe already created)
                var errorMessage = $"[User Updated Event] - Failed - User does not exist [User: {userFromEvent.PublicId}, Version: {userFromEvent.Version}]";
                logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }

            if ((userFromEvent.Version - 1) != existingUser.Version)
            {
                // schedule redelivery (user updates can get out of order)
                var errorMessage = $"[User Updated Event] - Failed - Version Mismatch [User: {userFromEvent.PublicId}, Version: {userFromEvent.Version}]";
                logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }
            
            // update existing user:
            existingUser.Email = userFromEvent.Email;
            existingUser.Firstname = userFromEvent.Firstname;
            existingUser.Lastname = userFromEvent.Lastname;
            existingUser.UserName = userFromEvent.UserName;
            existingUser.PublicId = userFromEvent.PublicId;
            existingUser.RefreshToken = userFromEvent.RefreshToken;
            existingUser.RoleId = userFromEvent.RoleId;
            existingUser.BannedAt = userFromEvent.BannedAt;
            existingUser.WarningsCount = userFromEvent.WarningsCount;
            existingUser.CreatedAt = userFromEvent.CreatedAt;
            existingUser.CreatedByUserId = userFromEvent.CreatedByUserId;
            existingUser.LastUpdatedAt = userFromEvent.LastUpdatedAt;
            existingUser.LastUpdatedByUserId = userFromEvent.LastUpdatedByUserId;
            existingUser.Version = userFromEvent.Version;

            await userRepo.Save(existingUser);
            logger.LogInformation($"[User Updated Event] - Processed - [User: {userFromEvent.PublicId}, Version: {userFromEvent.Version}]");
        }
    }
}
