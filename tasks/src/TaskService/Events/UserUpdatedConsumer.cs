using System;
using AutoMapper;
using Cashflow.Common.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using TaskService.Data.Models;
using TaskService.Data.Repos.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Events
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
            existingUser.UserName = userFromEvent.UserName;
            existingUser.PublicId = userFromEvent.PublicId;
            existingUser.Firstname = userFromEvent.Firstname;
            existingUser.Lastname = userFromEvent.Lastname;
            existingUser.RefreshToken = userFromEvent.RefreshToken;
            existingUser.Gender = userFromEvent.Gender;
            existingUser.RoleId = userFromEvent.RoleId;
            existingUser.IsBanned = userFromEvent.IsBanned;
            await userRepo.Save(existingUser);
            
            logger.LogInformation($"[User Updated Event] - Processed - [User: {userFromEvent.PublicId}, Version: {userFromEvent.Version}]");
        }
    }
}