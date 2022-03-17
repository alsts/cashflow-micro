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
    [Route("api/promotion/taskjobs")]
    public class PromotionTasksJobController : ControllerBase
    {
        private readonly ILogger<PromotionTasksJobController> logger;
        private readonly ITaskService taskService;
        private readonly ISendEndpointProvider sendEndpoint;
        private readonly IMapper mapper;

        public PromotionTasksJobController(ITaskService taskService, ISendEndpointProvider sendEndpoint, IMapper mapper)
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
        public async Task<IActionResult> GetById(
            [FromQuery(Name = "task")] string task,
            [FromQuery(Name = "user")] string user)
        {
            TasksQuery tasksQuery = new TasksQuery
            {
                Task = task,
                User = user
            };

            await taskService.GetJobsByQuery(tasksQuery);
            
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

    public class TasksQuery
    {
        public string Task { get; set; }
        public string User { get; set; }
    }
}
