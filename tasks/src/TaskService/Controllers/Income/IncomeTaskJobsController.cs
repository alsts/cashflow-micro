using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskService.Dtos.Income;
using TaskService.Services.Income.interfaces;

namespace TaskService.Controllers.Income
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/income/tasks")]
    public class IncomeTaskJobsController : ControllerBase
    {
        private readonly ILogger<IncomeTaskJobsController> logger;
        private readonly ITaskJobIncomeService taskJobsIncomeService;
        private readonly ISendEndpointProvider sendEndpoint;
        private readonly IMapper mapper;

        public IncomeTaskJobsController(
            ITaskJobIncomeService taskJobsIncomeService,
            ISendEndpointProvider sendEndpoint,
            IMapper mapper)
        {
            this.taskJobsIncomeService = taskJobsIncomeService;
            this.sendEndpoint = sendEndpoint;
            this.mapper = mapper;
        }

        [HttpGet("jobs")]
        public async Task<IActionResult> GetTaskJobsForUser()
        {
            var taskJobs = await taskJobsIncomeService.GetForCurrentUser();
            return Ok(taskJobs.Select(taskJob => mapper.Map<IncomeTaskJobDto>(taskJob)));
        }
        
        [HttpGet("jobs/{jobId}")]
        public async Task<IActionResult> GetTaskJobById(string jobId)
        {
            var taskJob = await taskJobsIncomeService.GetById(jobId);
            return Ok(mapper.Map<IncomeTaskJobDto>(taskJob));
        }
        
        [HttpGet("jobs/{jobId}/submit-report")]
        public async Task<IActionResult> SubmitReportForJob(
            [FromBody] TaskJobReportDto report,
            string jobId)
        {
            await taskJobsIncomeService.SubmitReport(jobId, report);
            return Ok();
        }
        
        [HttpGet("jobs/{jobId}/submit-improvement")]
        public async Task<IActionResult> SubmitReportImprovementForJob(
            [FromBody] TaskJobReportImprovementDto reportImprovement, 
            string jobId)
        {
            await taskJobsIncomeService.SubmitReportImprovement(jobId, reportImprovement);
            return Ok();
        }
        
        [HttpGet("jobs/{jobId}/cancel")]
        public async Task<IActionResult> CancelJob(string jobId)
        {
            await taskJobsIncomeService.Cancel(jobId);
            return Ok();
        }
        
        [HttpGet("{taskId}/jobs/can-start")]
        public async Task<IActionResult> CanUserStartWorkingOnTask(string taskId)
        {
            bool taskJob = await taskJobsIncomeService.CanUserStartWorkingOnTask(taskId);
            return Ok(mapper.Map<IncomeTaskJobDto>(taskJob));
        }
        
        [HttpPost("{taskId}/jobs")]
        public async Task<IActionResult> StartWorkingOnTask(string taskId)
        {
            var taskJob = await taskJobsIncomeService.StartWorkingOnTask(taskId);
            return Ok(mapper.Map<IncomeTaskJobDto>(taskJob));
        }
    }
}
