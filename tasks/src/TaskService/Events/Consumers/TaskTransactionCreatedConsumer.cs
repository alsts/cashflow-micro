using System;
using AutoMapper;
using Cashflow.Common.Events.Accounts;
using Cashflow.Common.Events.Money;
using MassTransit;
using Microsoft.Extensions.Logging;
using TaskService.Data.Models.External;
using TaskService.Data.Repos.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace TaskService.Events.Consumers
{
    public class TaskTransactionCreatedConsumer : IConsumer<TaskTransactionCreatedEvent>
    {
        private readonly IMapper mapper;
        private readonly ILogger<TaskTransactionCreatedConsumer> logger;
        private readonly ITaskTransactionRepo taskTransactionRepo;
        
        public TaskTransactionCreatedConsumer(
            IMapper mapper, 
            ILogger<TaskTransactionCreatedConsumer> logger, 
            ITaskTransactionRepo taskTransactionRepo)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.taskTransactionRepo = taskTransactionRepo;
        }

        public async Task Consume(ConsumeContext<TaskTransactionCreatedEvent> context)
        {
            var taskTransactionFromEvent = mapper.Map<TaskTransaction>(context.Message);

            if (taskTransactionFromEvent == null)
            {
                logger.LogError($"[TaskTransaction Created Event] - Failed - User event is null");
                return; // remove broken event from queue
            }
            
            if (await taskTransactionRepo.GetByPublicId(taskTransactionFromEvent.PublicId) != null)
            {
                var errorMessage = $"[TaskTransaction Created Event] - Failed - Transaction already exists [TaskTransaction: {taskTransactionFromEvent.PublicId}]";
                logger.LogError(errorMessage);
                throw new Exception(errorMessage);
            }
            
            await taskTransactionRepo.Save(taskTransactionFromEvent);
            logger.LogInformation($"[TaskTransaction Created Event] - Processed [TaskTransaction: {taskTransactionFromEvent.PublicId}, Version: {taskTransactionFromEvent.Version}]");
        }
    }
}
