using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModerationService.Services.interfaces;

namespace ModerationService.Controllers
{
    [ApiController]
    [Route("api/moderation")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> logger;
        private readonly IModerationService moderationService;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IMapper mapper;

        public AccountController(IModerationService moderationService, IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            this.moderationService = moderationService;
            this.publishEndpoint = publishEndpoint;
            this.mapper = mapper;
        }
        
        // TODO: ////////////////////
        // Get Tasks Awaiting Approval
        // Approve Task
        // Request Changes
        
        // Get List of Banned Users
        // Ban User
    }
}
