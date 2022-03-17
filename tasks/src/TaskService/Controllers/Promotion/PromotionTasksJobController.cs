using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskService.Data.Models;
using TaskService.Dtos.Promotion;
using TaskService.Services.Promotion.interfaces;

namespace TaskService.Controllers.Promotion
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/promotion/tasks")]
    public class PromotionTasksJobController : ControllerBase
    {
        private readonly ILogger<PromotionTasksJobController> logger;
        private readonly ITaskJobPromotionService taskJobPromotionService;
        private readonly IMapper mapper;

        public PromotionTasksJobController(
            ITaskJobPromotionService taskJobPromotionService, 
            IMapper mapper)
        {
            this.taskJobPromotionService = taskJobPromotionService;
            this.mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetJobsByQuery(
            [FromQuery(Name = "task")] string task,
            [FromQuery(Name = "user")] string user)
        {
            TasksQuery tasksQuery = new TasksQuery (task, user);
            IEnumerable<TaskJob> taskJobs =  await taskJobPromotionService.GetJobsByQuery(tasksQuery);
            return Ok(taskJobs.Select(taskJob => mapper.Map<PromotionTaskJobDto>(taskJob)));
        }
        
        [HttpGet("{jobId}")]
        public async Task<IActionResult> GetJobById(string jobId)
        {
            TaskJob taskJob =  await taskJobPromotionService.GetById(jobId);
            return Ok(mapper.Map<PromotionTaskJobDto>(taskJob));
        }

        [HttpPost("{jobId}/approve")]
        public async Task<IActionResult> Approve(string jobId)
        {
            await taskJobPromotionService.Approve(jobId);
            return Ok();
        }
        
        [HttpPost("{jobId}/decline")]
        public async Task<IActionResult> Decline(string jobId)
        {
            await taskJobPromotionService.Decline(jobId);
            return Ok();
        }
        
        [HttpPost("{jobId}/request-improvement")]
        public async Task<IActionResult> RequestImprovement(string jobId)
        {
            await taskJobPromotionService.RequestImprovement(jobId);
            return Ok();
        }
    }

    public class TasksQuery
    {
        public TasksQuery(string task, string user)
        {
            Task = task;
            User = user;
        }

        public string Task { get; set; }
        public string User { get; set; }
    }
}
