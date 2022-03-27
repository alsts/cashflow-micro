using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AccountService.Data;
using AccountService.Data.Models;
using AccountService.Data.Repos.Interfaces;
using AccountService.Dtos;
using AccountService.Services.interfaces;
using AccountService.Util;
using AccountService.Util.AccountService.Util.Helpers;
using Cashflow.Common.Data.DataObjects;
using Cashflow.Common.Data.Enums;
using Cashflow.Common.Exceptions;
using Cashflow.Common.Utils.Interfaces;

namespace AccountService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo userRepo;
        private readonly IPasswordHasher passwordHasher;
        private readonly LoggedInUserDataHolder loggedInUserDataHolder;

        public UserService(IUserRepo userRepo, IPasswordHasher passwordHasher, LoggedInUserDataHolder loggedInUserDataHolder)
        {
            this.userRepo = userRepo;
            this.passwordHasher = passwordHasher;
            this.loggedInUserDataHolder = loggedInUserDataHolder;
        }

        public async Task<User> SignUp(UserSignUpDto userSignUpDto)
        {
            if (userSignUpDto.Email == null || userSignUpDto.Username == null || userSignUpDto.Password == null)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Please fill all required fields");
            }

            var usernameRegex = new Regex(@"^[a-zA-Z][a-zA-Z0-9]{3,15}$");
            if (!usernameRegex.IsMatch(userSignUpDto.Username))
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Username does not match requirements");
            }

            if (!AuthUtils.IsPasswordValid(userSignUpDto.Password))
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Password does not match requirements");
            }
            
            if (!new EmailAddressAttribute().IsValid(userSignUpDto.Email))
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Need to specify valid email");
            }

            if (await userRepo.GetUserByEmail(userSignUpDto.Email) != null)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Email is already taken");
            }

            if (await userRepo.GetUserByUsername(userSignUpDto.Username) != null)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Username is already taken");
            }

            var hashedPassword = passwordHasher.Hash(userSignUpDto.Password);

            var user = new User
            {
                UserName = userSignUpDto.Username,
                Password = hashedPassword,
                Email = userSignUpDto.Email,
                Firstname = userSignUpDto.Firstname,
                Lastname = userSignUpDto.Lastname,
                Gender = (Genders)userSignUpDto.Gender,
                PublicId = Guid.NewGuid().ToString(),
                RefreshToken = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                RoleId = (int)Roles.User,
                IsActive = true
            };
            
            await userRepo.Save(user);

            user = await userRepo.GetUserByUsername(userSignUpDto.Username);
            if (user == null)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "User not created");
            }

            return user.WithoutPassword();
        }

        public async Task<User> SignIn(UserSignInDto userSignInDto)
        {
            var hashedPassword = passwordHasher.Hash(userSignInDto.Password);
            var user = await userRepo.GetUserByUsernameAndPassword(userSignInDto.Username, hashedPassword);

            if (user == null)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, "User with this username and password not found");
            }

            if (user.BannedAt != null)
            {
                throw new HttpStatusException(HttpStatusCode.Forbidden, "User is banned");
            }

            await UpdateRefreshTokenForUser(user);
            return user.WithoutPassword();
        }

        public async Task<User> GetCurrent()
        {
            if (String.IsNullOrEmpty(loggedInUserDataHolder.UserId) && String.IsNullOrEmpty(loggedInUserDataHolder.RefreshToken))
            {
                throw new HttpStatusException(HttpStatusCode.Unauthorized, "Invalid user");
            }

            var user = await userRepo.GetUserByPublicIdAndRefreshToken(loggedInUserDataHolder.UserId, loggedInUserDataHolder.RefreshToken);
            if (user == null)
            {
                throw new HttpStatusException(HttpStatusCode.Unauthorized, "User not found");
            }
            return user;
        }

        public async Task<User> Update(UserUpdateDto userUpdateDto)
        {
            var user = await GetCurrent();

            if (String.IsNullOrEmpty(userUpdateDto.Email) ||
                String.IsNullOrEmpty(userUpdateDto.Firstname) ||
                String.IsNullOrEmpty(userUpdateDto.Lastname))
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Please fill all required fields");
            }
            
            if (!new EmailAddressAttribute().IsValid(userUpdateDto.Email))
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Need to specify valid email");
            }

            var emailChanged = user.Email != userUpdateDto.Email;
            if (emailChanged && await userRepo.GetUserByEmail(userUpdateDto.Email) != null)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Email is already taken");
            }

            user.Email = userUpdateDto.Email;
            user.Firstname = userUpdateDto.Firstname;
            user.Lastname = userUpdateDto.Lastname;

            await userRepo.Save(user);
            return user.WithoutPassword();
        }

        public async Task<User> GetByPublicId(string id)
        {
            var user = await userRepo.GetByPublicId(id);
            if (user == null)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, "User not found");
            }
            return user.WithoutPassword();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await userRepo.GetAll();
        }

        public async Task UpdateRefreshTokenForUser(User user)
        {
            user.RefreshToken = Guid.NewGuid().ToString();
            await userRepo.Save(user);
        }
    }
}
