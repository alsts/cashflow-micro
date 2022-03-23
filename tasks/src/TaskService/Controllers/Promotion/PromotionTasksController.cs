using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskService.Dtos.Promotion;
using TaskService.Events.Publishers.Interfaces;
using TaskService.Services.Promotion.interfaces;

namespace TaskService.Controllers.Promotion
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/promotion/tasks")]
    public class PromotionTasksController : ControllerBase
    {
        private readonly ILogger<PromotionTasksController> logger;
        private readonly ITaskPromotionService taskPromotionService;
        private readonly IMessageBusPublisher messageBusPublisher;
        private readonly IMapper mapper;

        public PromotionTasksController(
            ITaskPromotionService taskPromotionService, 
            IMessageBusPublisher messageBusPublisher,
            IMapper mapper)
        {
            this.taskPromotionService = taskPromotionService;
            this.messageBusPublisher = messageBusPublisher;
            this.mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUserTasks()
        {
            var tasks = await taskPromotionService.GetForCurrentUser();
            return Ok(tasks.Select(task => mapper.Map<PromotionTaskDto>(task)));
        }
        
        [HttpGet("{publicId}")]
        public async Task<IActionResult> GetById(string publicId)
        {
            var task = await taskPromotionService.GetByPublicId(publicId);
            return Ok(mapper.Map<PromotionTaskDto>(task));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskCreateDto model)
        {
            if (!messageBusPublisher.IsEventBusHealthy())
            {
                return BadRequest();
            }
            
            var task = await taskPromotionService.Create(model);
            await messageBusPublisher.PublishCreatedTask(task);
            
            return Ok(mapper.Map<PromotionTaskDto>(task));
        }
        
        [HttpPut("{publicId}")]
        public async Task<IActionResult> Update([FromBody] TaskUpdateDto model, string publicId)
        {
            if (!messageBusPublisher.IsEventBusHealthy())
            {
                return BadRequest();
            }
            
            var task = await taskPromotionService.Update(model, publicId);
            await messageBusPublisher.PublishUpdatedTask(task);

            return Ok(mapper.Map<PromotionTaskDto>(task));
        }
        
        [HttpPost("{publicId}/start")]
        public async Task<IActionResult> StartTask(string publicId)
        {
            await taskPromotionService.StartTask(publicId);
            return Ok();
        }
        
        [HttpPost("{publicId}/stop")]
        public async Task<IActionResult> StopTask(string publicId)
        {
            await taskPromotionService.StopTask(publicId);
            return Ok();
        }
    }
}
