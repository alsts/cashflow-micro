using System.Linq;
using System.Threading.Tasks;
using AccountService.Dtos;
using AccountService.Services.interfaces;
using AccountService.Util.Enums;
using AccountService.Util.Helpers;
using AccountService.Util.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccountService.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> logger;
        private readonly JwtTokenCreator jwtCreator;
        private readonly IUserService userService;

        public AccountController(JwtTokenCreator jwtCreator, IUserService userService)
        {
            this.jwtCreator = jwtCreator;
            this.userService = userService;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] UserSignInDto model)
        {
            var user = await userService.SignIn(model);
            var token = jwtCreator.GenerateForUser(user);
            Response.AppendAuthCookies(user, token);
            return Ok(user.ToPublicDto());
        }
        
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserSignUpDto model)
        {
            var user = await userService.SignUp(model);
            var token = jwtCreator.GenerateForUser(user);
            Response.AppendAuthCookies(user, token);
            return Ok(user.ToPublicDto());
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            Response.AppendAuthCookies(user, token);
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
