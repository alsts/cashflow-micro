using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cashflow.Common.Data.Enums;
using Cashflow.Common.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModerationService.Events.Publishers.Interfaces;
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
        private readonly IMessageBusPublisher messageBusPublisher;
        private readonly IMapper mapper;

        public TasksModerationController(
            ITaskModerationService tasksModerationService, 
            IMessageBusPublisher messageBusPublisher,
            IMapper mapper)
        {
            this.tasksModerationService = tasksModerationService;
            this.messageBusPublisher = messageBusPublisher;
            this.mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetTasksToModerate()
        {
            var tasks = await tasksModerationService.GetTasksToModerate();
            return Ok(tasks.Select(t => t.ToPublicDto()));
        }
        
        [HttpPost("{taskId}/approve")]
        public async Task<IActionResult> ApproveTask(string taskId)
        {
            if (!messageBusPublisher.IsEventBusHealthy())
            {
                return BadRequest();
            }
            
            var task = await tasksModerationService.ApproveTask(taskId);
            await messageBusPublisher.PublishTaskApprovedEvent(task);
            
            return Ok(task.ToPublicDto());
        }
    }
}
