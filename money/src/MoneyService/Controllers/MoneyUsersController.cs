using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoneyService.Services.interfaces;

namespace MoneyService.Controllers
{
    [ApiController]
    [Route("api/moder")]
    public class UsersMoneyController : ControllerBase
    {
        private readonly ILogger<UsersMoneyController> logger;
        private readonly IMoneyService _moneyService;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IMapper mapper;

        public UsersMoneyController(IMoneyService moneyService, IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            this._moneyService = moneyService;
            this.publishEndpoint = publishEndpoint;
            this.mapper = mapper;
        }
        
    }
}
