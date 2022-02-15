using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoneyService.Services.interfaces;

namespace MoneyService.Controllers
{
    [ApiController]
    [Route("api/moder")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> logger;
        private readonly IMoneyService _moneyService;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IMapper mapper;

        public AccountController(IMoneyService moneyService, IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            this._moneyService = moneyService;
            this.publishEndpoint = publishEndpoint;
            this.mapper = mapper;
        }
        
        // TODO: ////////////////////
        // Get Tasks Awaiting Approval
        // Approve Task
        // Request Changes
        
        // Get List of Banned Users
        // Ban User
    }
}
