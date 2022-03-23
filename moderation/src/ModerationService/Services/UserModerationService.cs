using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Cashflow.Common.Data.DataObjects;
using Cashflow.Common.Exceptions;
using ModerationService.Data.Models.External;
using ModerationService.Data.Repos.Interfaces;
using ModerationService.Services.interfaces;
using TaskEntity = ModerationService.Data.Models.External.Task;

namespace ModerationService.Services
{
    public class UserModerationService : IUserModerationService
    {
        private readonly ITaskRepo taskRepo;
        private readonly IUserRepo userRepo;
        private readonly LoggedInUserDataHolder loggedInUserDataHolder;

        public UserModerationService(
            ITaskRepo taskRepo,
            IUserRepo userRepo,
            LoggedInUserDataHolder loggedInUserDataHolder)
        {
            this.taskRepo = taskRepo;
            this.userRepo = userRepo;
            this.loggedInUserDataHolder = loggedInUserDataHolder;
        }

        public async Task<IEnumerable<User>> GetUsersToModerate()
        {
            return await userRepo.GetUsersToModerate();
        }

        public async Task<User> BanUser(string userId)
        {
            var user = await userRepo.GetByPublicId(userId);
            if (user == null)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, "User not found");
            }
            
            user.BannedAt = DateTime.Now;
            user.IsActive = false;
            await userRepo.Save(user);
            
            return user;
        }
    }
}
