using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cashflow.Common.Data.Enums;
using Cashflow.Common.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModerationService.Dtos;
using ModerationService.Events.Publishers.Interfaces;
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
        private readonly IMessageBusPublisher messageBusPublisher;
        private readonly IMapper mapper;

        public UsersModerationController(
            IUserModerationService userModerationService, 
            IMessageBusPublisher messageBusPublisher,
            IMapper mapper)
        {
            this.userModerationService = userModerationService;
            this.messageBusPublisher = messageBusPublisher;
            this.mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUsersToModerate()
        {
            var tasks = await userModerationService.GetUsersToModerate();
            return Ok(tasks.Select(t => t.ToPublicDto()));
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("{userId}/ban")]
        public async Task<IActionResult> BanUser(string userId)
        {
            if (!messageBusPublisher.IsEventBusHealthy())
            {
                return BadRequest();
            }
            
            var bannedUser = await userModerationService.BanUser(userId);
            await messageBusPublisher.PublishUserBannedEvent(bannedUser);
            
            return Ok(mapper.Map<UserBanDto>(bannedUser));
        }
    }
}
