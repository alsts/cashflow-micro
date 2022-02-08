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
using TaskService.Dtos;
using TaskService.Services.interfaces;
using TaskService.Util;

namespace TaskService.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> logger;
        private readonly ITaskService taskService;
        private readonly ISendEndpointProvider sendEndpoint;
        private readonly IMapper mapper;

        public AccountController(ITaskService taskService, ISendEndpointProvider sendEndpoint, IMapper mapper)
        {
            this.taskService = taskService;
            this.sendEndpoint = sendEndpoint;
            this.mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] TaskCreateDto model)
        {
            var task = await taskService.Create(model);
            
            // return CreatedAtRoute(nameof(SignUp), new { Id = user.PublicId }, user.ToPublicDto());
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
