using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoneyService.Services.interfaces;

namespace MoneyService.Controllers
{
    [ApiController]
    [Route("api/money/tasks")]
    public class MoneyTasksController : ControllerBase
    {
        private readonly ILogger<MoneyTasksController> logger;
        private readonly IMoneyService _moneyService;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IMapper mapper;

        public MoneyTasksController(IMoneyService moneyService, IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            this._moneyService = moneyService;
            this.publishEndpoint = publishEndpoint;
            this.mapper = mapper;
        }
        
    }
}
