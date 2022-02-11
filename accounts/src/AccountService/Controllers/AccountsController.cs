using System.Linq;
using System.Threading.Tasks;
using AccountService.Dtos;
using AccountService.Events;
using AccountService.Services.interfaces;
using AccountService.Util.AccountService.Util.Helpers;
using AccountService.Util.Jwt;
using Cashflow.Common.Data.Enums;
using Cashflow.Common.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> logger;
        private readonly JwtTokenCreator jwtCreator;
        private readonly IUserService userService;
        private readonly MessageBusPublisher messageBusPublisher;

        public AccountController(
            JwtTokenCreator jwtCreator,
            IUserService userService, 
            MessageBusPublisher messageBusPublisher)
        {
            this.jwtCreator = jwtCreator;
            this.userService = userService;
            this.messageBusPublisher = messageBusPublisher;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInDto model)
        {
            var user = await userService.SignIn(model);
            var token = jwtCreator.GenerateForUser(user);
            Response.AppendAuthCookie(user, token);
            return Ok(user.ToPublicDto());
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpDto model)
        {
            if (!messageBusPublisher.IsEventBusHealthy())
            {
                return BadRequest();
            }

            var user = await userService.SignUp(model);
            await messageBusPublisher.PublishCreatedUser(user);

            var token = jwtCreator.GenerateForUser(user);
            Response.AppendAuthCookie(user, token); 

            return Ok(user.ToPublicDto());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var user = await userService.GetCurrent();
            await userService.UpdateRefreshTokenForUser(user);
            var token = jwtCreator.GenerateForUser(user);
            Response.AppendAuthCookie(user, token);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("")]
        public async Task<IActionResult> GetCurrent()
        {
            var user = await userService.GetCurrent();
            return Ok(user.ToPublicDto());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("")]
        public async Task<IActionResult> Update([FromBody] UserUpdateDto model)
        {
            var user = await userService.Update(model);
            await messageBusPublisher.PublishUpdatedUser(user);
            
            return Ok(user.ToPublicDto());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{publicId}")]
        public async Task<IActionResult> GetById(string publicId)
        {
            var user = await userService.GetByPublicId(publicId);
            return Ok(user.ToPublicDto());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [AuthorizeRoles(Roles.Admin, Roles.SuperAdmin)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var user = await userService.GetAll();
            return Ok(user.Select(u => u.ToPublicDto()));
        }
    }
}
