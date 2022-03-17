using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoneyService.Data.Models;
using MoneyService.Events.Publishers.Interfaces;
using MoneyService.Services.interfaces;

namespace MoneyService.Controllers
{
    [ApiController]
    [Route("api/money/users")]
    public class MoneyUsersController : ControllerBase
    {
        private readonly ILogger<MoneyUsersController> logger;
        private readonly IMoneyUsersService moneyUsersService;
        private readonly IMessageBusPublisher messageBusPublisher;
        private readonly IMapper mapper;

        public MoneyUsersController(
            IMoneyUsersService moneyUsersService,
            IMessageBusPublisher messageBusPublisher,
            IMapper mapper)
        {
            this.moneyUsersService = moneyUsersService;
            this.messageBusPublisher = messageBusPublisher;
            this.mapper = mapper;
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetMainBalanceForUser()
        {
            var userMainBalance = await moneyUsersService.GetMainBalanceForUser();
            return Ok(userMainBalance);
        }
        
        [HttpPost("balance/deposit")]
        public async Task<IActionResult> DepositToMainBalance(decimal amount)
        {
            if (!messageBusPublisher.IsEventBusHealthy())
            {
                return BadRequest();
            }
            
            UserTransaction userTransaction = await moneyUsersService.DepositToUserMainBalance(amount);
            await messageBusPublisher.PublishCreatedUserTransaction(userTransaction);
            
            return Ok();
        }

        [HttpPost("balance/withdraw")]
        public async Task<IActionResult> WithdrawFromMainBalance(decimal amount)
        {
            if (!messageBusPublisher.IsEventBusHealthy())
            {
                return BadRequest();
            }
            
            UserTransaction userWithdrawalTransaction = await moneyUsersService.WithdrawFromMainBalance(amount);
            await messageBusPublisher.PublishCreatedUserTransaction(userWithdrawalTransaction);
            
            return Ok();
        }

        [HttpGet("ad-balance")]
        public async Task<IActionResult> GetAdBalanceForUser()
        {
            var userAdBalance = await moneyUsersService.GetAdBalanceForCurrentUser();
            return Ok(userAdBalance);
        }

        [HttpPost("ad-balance/add")]
        public async Task<IActionResult> AddMoneyToAdBalance(decimal amount)
        {
            if (!messageBusPublisher.IsEventBusHealthy())
            {
                return BadRequest();
            }
            
            (UserTransaction userMainBalanceTransaction, 
                UserTransaction userAdBalanceTransaction) = await moneyUsersService.AddMoneyToAdBalance(amount);

            await messageBusPublisher.PublishCreatedUserTransaction(userMainBalanceTransaction);
            await messageBusPublisher.PublishCreatedUserTransaction(userAdBalanceTransaction);
            
            return Ok();
        }
    }
}
