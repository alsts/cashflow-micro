using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo repository;
        private readonly IMapper mapper;

        public CommandsController(ICommandRepo repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        
        [HttpGet("platform/{platformId}")]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatformId(int platformId)
        {
            Console.WriteLine($"--> Get Commands For Platform: {platformId}");

            if (!repository.PlatformExist(platformId))
            {
                return NotFound();
            }

            var commands = repository.GetCommandsForPlatform(platformId);
            return Ok(mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }
        
        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<PlatformReadDto> GetCommandById(int id)
        {
            Console.WriteLine("---> Get Command By Id");
            var command = repository.GetCommand(id);
            if (command == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<CommandReadDto>(command));
        }
        
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            Console.WriteLine("--> Create Command");
            
            if (!repository.PlatformExist(commandCreateDto.PlatformId))
            {
                return NotFound();
            }

            var command = mapper.Map<Command>(commandCreateDto);
            repository.CreateCommand(command);
            repository.SaveChanges();

            var commandReadDto = mapper.Map<CommandReadDto>(command);
            return CreatedAtRoute(nameof(GetCommandById), new {Id = commandReadDto.PlatformId}, commandReadDto);
        }
 
        [HttpPost("test")]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");
            return Ok("Test");
        }
    }
}
