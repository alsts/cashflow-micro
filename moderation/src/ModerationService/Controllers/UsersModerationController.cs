using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cashflow.Common.Data.Enums;
using Cashflow.Common.Utils;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModerationService.Services.interfaces;
using ModerationService.Util.AccountService.Util.Helpers;

namespace ModerationService.Controllers
{
    [ApiController]
    [AuthorizeRoles(Roles.Admin, Roles.SuperAdmin)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/moderation/users")]
    public class UsersModerationController : ControllerBase
    {
        private readonly ILogger<UsersModerationController> logger;
        private readonly IUserModerationService userModerationService;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IMapper mapper;

        public UsersModerationController(
            IUserModerationService userModerationService, 
            IPublishEndpoint publishEndpoint, 
            IMapper mapper)
        {
            this.userModerationService = userModerationService;
            this.publishEndpoint = publishEndpoint;
            this.mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUsersToModerate()
        {
            var tasks = await userModerationService.GetUsersToModerate();
            return Ok(tasks.Select(t => t.ToPublicDto()));
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{userId}/ban")]
        public async Task<IActionResult> BanUser(string userId)
        {
            var task = await userModerationService.BanUser(userId);
            return Ok(task.ToPublicDto());
        }
    }
}
