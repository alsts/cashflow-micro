using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoneyService.Data.Models;
using MoneyService.Dtos;
using MoneyService.Events.Publishers.Interfaces;
using MoneyService.Services.interfaces;

namespace MoneyService.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/money/tasks")]
    public class MoneyTasksController : ControllerBase
    {
        private readonly ILogger<MoneyTasksController> logger;
        private readonly IMoneyTasksService moneyTasksService;
        private readonly IMessageBusPublisher messageBusPublisher;
        private readonly IMapper mapper;

        public MoneyTasksController(
            IMoneyTasksService moneyTasksService, 
            IMessageBusPublisher messageBusPublisher, 
            IMapper mapper)
        {
            this.moneyTasksService = moneyTasksService;
            this.messageBusPublisher = messageBusPublisher;
            this.mapper = mapper;
        }
        
        [HttpGet("{taskId}/balance")]
        public async Task<IActionResult> GetById(string taskId)
        {
            decimal amount = await moneyTasksService.GetTaskBalance(taskId);
            return Ok(amount);
        }
        
        [HttpPost("{taskId}/balance/add/{amount}")]
        public async Task<IActionResult> AddMoneyToTaskBalance(string taskId, decimal amount)
        {
            if (!messageBusPublisher.IsEventBusHealthy())
            {
                return BadRequest();
            }
            
            (UserTransaction userTransaction, 
                TaskTransaction taskTransaction) = await moneyTasksService.AddMoneyToTaskBalance(taskId, amount);
            
            await messageBusPublisher.PublishCreatedUserTransaction(userTransaction);
            await messageBusPublisher.PublishCreatedTaskTransaction(taskTransaction);
            
            return Ok();
        }
        
        [HttpPost("{taskId}/balance/return")]
        public async Task<IActionResult> ReturnMoneyFromTaskBalance(string taskId)
        {
            (UserTransaction userTransaction, 
                TaskTransaction taskTransaction) = await moneyTasksService.ReturnMoneyFromTaskBalance(taskId);
            
            await messageBusPublisher.PublishCreatedUserTransaction(userTransaction);
            await messageBusPublisher.PublishCreatedTaskTransaction(taskTransaction);
            
            return Ok();
        }
        
        [HttpGet("history")]
        public async Task<IActionResult> GetTransactionsHistory()
        {
            List<TaskTransaction> transactionsHistory = await moneyTasksService.GetTaskTransactionsHistoryForCurrentUser();
            return Ok(transactionsHistory.Select(t => mapper.Map<TaskTransactionReadDto>(t)));
        }
    }
}
