using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AccountService.Data;
using AccountService.Dtos;
using AccountService.Models;
using AccountService.Services.interfaces;
using AccountService.Util;
using AccountService.Util.Enums;
using AccountService.Util.Helpers;
using AccountService.Util.Helpers.Interfaces;
using Utils = AccountService.Util.Helpers.Utils;

namespace AccountService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo userRepo;
        private readonly IPasswordHasher passwordHasher;

        public UserService(IUserRepo userRepo, IPasswordHasher passwordHasher)
        {
            this.userRepo = userRepo;
            this.passwordHasher = passwordHasher;
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

            if (!Utils.IsPasswordValid(userSignUpDto.Password))
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Password does not match requirements");
            }

            if (await userRepo.GetUserByEmail(userSignUpDto.Email) != null)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "You already have an account, please log in");
            }
            
            if (await userRepo.GetUserByUsername(userSignUpDto.Username) != null)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "You already have an account, please log in");
            }

            if (!new EmailAddressAttribute().IsValid(userSignUpDto.Email))
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Need to specify valid email");
            }
            
            var hashedPassword = passwordHasher.Hash(userSignUpDto.Password);
            
            var user = new User
            {
                UserName = userSignUpDto.Username,
                Password = hashedPassword,
                Email = userSignUpDto.Email,
                Firstname = userSignUpDto.Firstname,
                Lastname = userSignUpDto.Surname,
                Gender = (Genders) userSignUpDto.Gender,
                RoleId = (int) Roles.User,
                IsActive = true
            };

            await userRepo.Save(user);

            user = await userRepo.GetUserByUsername(userSignUpDto.Username);

            if (user == null)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "User not created");
            }
            
            await UpdateRefreshToken(user);
            return user.WithoutPassword();
        }

        public async Task<User> SignIn(UserSignInDto userSignInDto)
        {
            var hashedPassword = passwordHasher.Hash(userSignInDto.Password);
            var user = await userRepo.GetUserByUsernameAndPassword(userSignInDto.Username, hashedPassword);

            if (user == null)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, "User with this email and password not found");
            }
            
            await UpdateRefreshToken(user);
            return user.WithoutPassword();
        }

        public async Task<User> GetUserByUsernameAndRefreshToken(string userName, string refreshToken)
        {
            return await userRepo.GetUserByUsernameAndRefreshToken(userName, refreshToken);
        }

        public async Task UpdateRefreshToken(User user)
        {
            user.RefreshToken = Guid.NewGuid().ToString();
            await userRepo.Save(user);
        }
    }
}
