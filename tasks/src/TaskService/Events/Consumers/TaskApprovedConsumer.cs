using System;
using AutoMapper;
using Cashflow.Common.Data.Enums;
using Cashflow.Common.Events.Moderation;
using MassTransit;
using Microsoft.Extensions.Logging;
using TaskService.Data.Models.External;
using TaskService.Data.Repos.Interfaces;
using TaskService.Events.Publishers.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Events.Consumers
{
    public class TaskApprovedConsumer : IConsumer<TaskApprovedEvent>
    {
        private readonly ILogger<TaskApprovedConsumer> logger;
        private readonly ITaskRepo taskRepo;
        private readonly IMessageBusPublisher messageBusPublisher;
        
        public TaskApprovedConsumer(
            ILogger<TaskApprovedConsumer> logger, 
            IMessageBusPublisher messageBusPublisher,
            ITaskRepo taskRepo)
        {
            this.logger = logger;
            this.taskRepo = taskRepo;
            this.messageBusPublisher = messageBusPublisher;
        }

        public async Task Consume(ConsumeContext<TaskApprovedEvent> context)
        {
            TaskApprovedEvent taskApprovedEvent = context.Message;

            if (taskApprovedEvent == null)
            {
                logger.LogError($"[Task Approved Event] - Failed - Task Approved event is null");
                return; // remove broken event from queue
            }

            var existingTask = await taskRepo.GetByPublicId(taskApprovedEvent.TaskId);
            
            if (existingTask == null)
            {
                var errorMessage = $"[Task Approved Event] - Failed - Task does not exist [Task: {taskApprovedEvent.TaskId}]";
                logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }

            existingTask.TaskStatus = TaskStatus.Stopped;
            await taskRepo.Save(existingTask);
            
            logger.LogInformation($"[Task Approved Event] - Processed [Task: {existingTask.PublicId}]");
            
            // publish task updated event:
            await messageBusPublisher.PublishUpdatedTask(existingTask);
        }
    }
}
