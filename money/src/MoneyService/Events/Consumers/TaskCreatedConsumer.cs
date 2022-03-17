using System;
using AutoMapper;
using Cashflow.Common.Events.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using MoneyService.Data.Repos.Interfaces;
using Task = System.Threading.Tasks.Task;
using TaskEntity = MoneyService.Data.Models.Task;

namespace MoneyService.Events.Consumers
{
    public class TaskCreatedConsumer : IConsumer<TaskCreatedEvent>
    {
        private readonly IMapper mapper;
        private readonly ILogger<TaskCreatedConsumer> logger;
        private readonly ITaskRepo taskRepo;
        
        public TaskCreatedConsumer(IMapper mapper, ILogger<TaskCreatedConsumer> logger, ITaskRepo taskRepo)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.taskRepo = taskRepo;
        }

        public async Task Consume(ConsumeContext<TaskCreatedEvent> context)
        {
            var taskFromEvent = mapper.Map<TaskEntity>(context.Message);

            if (taskFromEvent == null)
            {
                logger.LogError($"[Task Created Event] - Failed - Task event is null");
                return; // remove broken event from queue
            }
            
            if (await taskRepo.GetByPublicId(taskFromEvent.PublicId) != null)
            {
                var errorMessage = $"[Task Created Event] - Failed - Task already exists [Task: {taskFromEvent.PublicId}]";
                logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }
            
            await taskRepo.Save(taskFromEvent);
            logger.LogInformation($"[Task Created Event] - Processed [Task: {taskFromEvent.PublicId}, Version: {taskFromEvent.Version}]");
        }
    }
}
