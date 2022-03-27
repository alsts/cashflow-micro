using System;
using System.Net;
using System.Threading.Tasks;
using Cashflow.Common.Data.DataObjects;
using Cashflow.Common.Exceptions;
using MoneyService.Data.Models;
using MoneyService.Data.Models.External;
using MoneyService.Data.Repos.Interfaces;
using MoneyService.Services.interfaces;

namespace MoneyService.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepo userRepo;
        private readonly LoggedInUserDataHolder loggedInUserDataHolder;
        
        public UsersService(
            IUserRepo userRepo,
            LoggedInUserDataHolder loggedInUserDataHolder)
        {
            this.userRepo = userRepo;
            this.loggedInUserDataHolder = loggedInUserDataHolder;
        }
        
        public async Task<User> GetCurrentUser()
        {
            if (String.IsNullOrEmpty(loggedInUserDataHolder.UserId) && String.IsNullOrEmpty(loggedInUserDataHolder.RefreshToken))
            {
                throw new HttpStatusException(HttpStatusCode.Unauthorized, "Invalid user");
            }

            var user = await userRepo.GetByPublicId(loggedInUserDataHolder.UserId);
            if (user == null)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, "User does not exist");
            }

            return user;
        }
    }
}
