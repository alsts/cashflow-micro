using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Cashflow.Common.Data.DataObjects;
using Cashflow.Common.Exceptions;
using TaskService.Data.Models;
using TaskService.Data.Models.External;
using TaskService.Data.Repos.Interfaces;
using TaskService.Services.General.interfaces;

namespace TaskService.Services.General
{
    public class UserService : IUserService
    {
        private readonly IUserRepo userRepo;
        private readonly LoggedInUserDataHolder loggedInUserDataHolder;

        public UserService(IUserRepo userRepo, LoggedInUserDataHolder loggedInUserDataHolder)
        {
            this.userRepo = userRepo;
            this.loggedInUserDataHolder = loggedInUserDataHolder;
        }

        public async Task<User> GetCurrent()
        {
            if (String.IsNullOrEmpty(loggedInUserDataHolder.UserID) && String.IsNullOrEmpty(loggedInUserDataHolder.RefreshToken))
            {
                throw new HttpStatusException(HttpStatusCode.Unauthorized, "Invalid user");
            }

            var user = await userRepo.GetUserByPublicIdAndRefreshToken(loggedInUserDataHolder.UserID, loggedInUserDataHolder.RefreshToken);
            if (user == null)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "User not found");
            }
            return user;
        }
        
        public async Task<User> GetByPublicId(string id)
        {
            var user = await userRepo.GetByPublicId(id);
            if (user == null)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, "User not found");
            }
            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await userRepo.GetAll();
        }
    }
}
