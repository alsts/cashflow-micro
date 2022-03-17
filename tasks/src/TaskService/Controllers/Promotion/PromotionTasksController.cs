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

namespace TaskService.Controllers.Promotion
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/promotion/tasks")]
    public class PromotionTasksController : ControllerBase
    {
        private readonly ILogger<PromotionTasksController> logger;
        private readonly ITaskService taskService;
        private readonly ISendEndpointProvider sendEndpoint;
        private readonly IMapper mapper;

        public PromotionTasksController(ITaskService taskService, ISendEndpointProvider sendEndpoint, IMapper mapper)
        {
            this.taskService = taskService;
            this.sendEndpoint = sendEndpoint;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskCreateDto model)
        {
            var task = await taskService.Create(model);
            
            // return CreatedAtRoute(nameof(SignUp), new { Id = user.PublicId }, user.ToPublicDto());
            return Ok(task.ToPublicDto());
        }
        
        [HttpPut("{publicId}")]
        public async Task<IActionResult> Update([FromBody] TaskUpdateDto model, string publicId)
        {
            var task = await taskService.Update(model, publicId);
            return Ok(task.ToPublicDto());
        }

        [HttpGet("{publicId}")]
        public async Task<IActionResult> GetById(string publicId)
        {
            var task = await taskService.GetByPublicId(publicId);
            return Ok(task.ToPublicDto());
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUserTasks()
        {
            var tasks = await taskService.GetForCurrentUser();
            return Ok(tasks.Select(t => t.ToPublicDto()));
        }
        
        [HttpPost("{publicId}/start")]
        public async Task<IActionResult> StartTask(string publicId)
        {
            await taskService.StartTask(publicId);
            return Ok();
        }
        
        [HttpPost("{publicId}/stop")]
        public async Task<IActionResult> StopTask(string publicId)
        {
            await taskService.StopTask(publicId);
            return Ok();
        }

        [AuthorizeRoles(Roles.Admin, Roles.SuperAdmin)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await taskService.GetAll();
            return Ok(tasks.Select(t => t.ToPublicDto()));
        }
    }
}
