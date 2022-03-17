using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskService.Services.Income.interfaces;
using TaskService.Services.interfaces;

namespace TaskService.Controllers.Income
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/income/tasks")]
    public class IncomeTasksController : ControllerBase
    {
        private readonly ILogger<IncomeTasksController> logger;
        private readonly ITaskIncomeService taskIncomeService;
        private readonly IMapper mapper;

        public IncomeTasksController(
            ITaskIncomeService taskIncomeService, 
            IMapper mapper)
        {
            this.taskIncomeService = taskIncomeService;
            this.mapper = mapper;
        }

        [HttpGet("{publicId}")]
        public async Task<IActionResult> GetById(string publicId)
        {
            var incomeTask = await taskIncomeService.GetByPublicId(publicId);
            return Ok(mapper.Map<IncomeTaskDto>(incomeTask));
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableTasks()
        {
            var availableTasks = await taskIncomeService.GetAvailableTasks();
            return Ok(availableTasks.Select(task => mapper.Map<IncomeTaskDto>(task)));
        }
    }

    public class IncomeTaskDto
    {
        public string PublicId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public int TaskStatus { get; set; }
        public int Version { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
