using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskService.Dtos;
using TaskService.Services.interfaces;
using TaskService.Util.Enums;
using TaskService.Util.Helpers;

namespace TaskService.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> logger;
        private readonly ITaskService taskService;

        public AccountController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] TaskCreateDto model)
        {
            var task = await taskService.Create(model);
            return Ok(task.ToPublicDto());
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{publicId}")]
        public async Task<IActionResult> Update([FromBody] TaskUpdateDto model, string publicId)
        {
            var task = await taskService.Update(model, publicId);
            return Ok(task.ToPublicDto());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{publicId}")]
        public async Task<IActionResult> GetById(string publicId)
        {
            var task = await taskService.GetByPublicId(publicId);
            return Ok(task.ToPublicDto());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [AuthorizeRoles(Roles.Admin, Roles.SuperAdmin)]
        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await taskService.GetAll();
            return Ok(tasks.Select(t => t.ToPublicDto()));
        }
    }
}
