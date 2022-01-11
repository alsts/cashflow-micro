using System;
using System.Threading.Tasks;
using AccountService.Dtos;
using AccountService.Models;
using AccountService.Services.interfaces;
using AccountService.Util.Enums;
using AccountService.Util.Helpers;
using AccountService.Util.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccountService.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> logger;
        private readonly JwtTokenCreator jwtCreator;
        private readonly IUserService userService;

        public AccountController(ILogger<AccountController> logger, JwtTokenCreator jwtCreator, IUserService userService)
        {
            this.logger = logger;
            this.jwtCreator = jwtCreator;
            this.userService = userService;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> LoginApi([FromBody] UserSignInDto model)
        {
            var user = await userService.SignIn(model);
            var token = jwtCreator.GenerateForUser(user);
            logger.LogInformation("User sign in");
            AppendAuthCookies(user, token);
            return Ok(user);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("ping")]
        public string Ping()
        {
            return "pong";
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [AuthorizeRoles(Roles.Admin, Roles.SuperAdmin)]
        [HttpGet("ping2")]
        public string Ping2() => "pong";

        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (!(Request.Cookies.TryGetValue("X-Username", out var userName) && Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshToken)))
                return BadRequest();

            var user = await userService.GetUserByUsernameAndRefreshToken(userName, refreshToken);
            if (user == null)
            {
                return BadRequest();
            }

            var token = jwtCreator.GenerateForUser(user);
            await userService.UpdateRefreshToken(user);

            AppendAuthCookies(user, token);
            return Ok();
        }

        private void AppendAuthCookies(User user, string token)
        {
            Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append("X-Username", user.UserName, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append("X-Refresh-Token", user.RefreshToken, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
        }
    }
}
