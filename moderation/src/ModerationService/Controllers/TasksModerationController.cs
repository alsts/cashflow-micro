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
    [Route("api/moderation/tasks")]
    public class TasksModerationController : ControllerBase
    {
        private readonly ILogger<TasksModerationController> logger;
        private readonly ITaskModerationService tasksModerationService;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IMapper mapper;

        public TasksModerationController(
            ITaskModerationService tasksModerationService, 
            IPublishEndpoint publishEndpoint, 
            IMapper mapper)
        {
            this.tasksModerationService = tasksModerationService;
            this.publishEndpoint = publishEndpoint;
            this.mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetTasksToModerate()
        {
            var tasks = await tasksModerationService.GetTasksToModerate();
            return Ok(tasks.Select(t => t.ToPublicDto()));
        }
        
        [HttpPut("{taskId}/approve")]
        public async Task<IActionResult> ApproveTask(string taskId)
        {
            var task = await tasksModerationService.ApproveTask(taskId);
            await messageBusPublisher.PublishUpdatedUser(user);
            
            return Ok(task.ToPublicDto());
        }
    }
}
