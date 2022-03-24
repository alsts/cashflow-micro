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

        public MoneyUsersController(IMoneyUsersService moneyUsersService)
        {
            this.moneyUsersService = moneyUsersService;
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
            await moneyUsersService.DepositToUserMainBalance(amount);
            return Ok();
        }

        [HttpPost("balance/withdraw")]
        public async Task<IActionResult> WithdrawFromMainBalance(decimal amount)
        {
            await moneyUsersService.WithdrawFromMainBalance(amount);
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
            await moneyUsersService.AddMoneyToAdBalance(amount);
            return Ok();
        }
    }
}
