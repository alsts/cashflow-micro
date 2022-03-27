using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoneyService.Dtos;
using MoneyService.Services.interfaces;

namespace MoneyService.Controllers
{
    [ApiController]
    [Route("api/money/users")]
    public class MoneyUsersController : ControllerBase
    {
        private readonly ILogger<MoneyUsersController> logger;
        private readonly IMoneyUsersService moneyUsersService;
        private readonly IMapper mapper;

        public MoneyUsersController(IMoneyUsersService moneyUsersService, IMapper mapper)
        {
            this.moneyUsersService = moneyUsersService;
            this.mapper = mapper;
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetMainBalanceForUser()
        {
            var userMainBalance = await moneyUsersService.GetMainBalanceForUser();
            return Ok(userMainBalance);
        }
        
        [HttpPost("balance/deposit/{amount}")]
        public async Task<IActionResult> DepositToMainBalance(decimal amount)
        {
            var depositTransaction = await moneyUsersService.DepositToUserMainBalance(amount);
            return Ok(mapper.Map<UserTransactionReadDto>(depositTransaction));
        }

        [HttpPost("balance/withdraw/{amount}")]
        public async Task<IActionResult> WithdrawFromMainBalance(decimal amount)
        {
            var withdrawTransaction = await moneyUsersService.WithdrawFromMainBalance(amount);
            return Ok(mapper.Map<UserTransactionReadDto>(withdrawTransaction));
        }

        [HttpGet("ad-balance")]
        public async Task<IActionResult> GetAdBalanceForUser()
        {
            var userAdBalance = await moneyUsersService.GetAdBalanceForCurrentUser();
            return Ok(userAdBalance);
        }

        [HttpPost("ad-balance/add/{amount}")]
        public async Task<IActionResult> AddMoneyToAdBalance(decimal amount)
        {
            await moneyUsersService.AddMoneyToAdBalance(amount);
            return Ok();
        }
    }
}
