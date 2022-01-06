using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.SyncData;
using PlatformServices.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo platformRepo;
        private readonly IMapper mapper;
        private readonly ICommandDataClient commandDataClient;
        private readonly IMessageBusClient messageBusClient;

        public PlatformsController(
            IPlatformRepo platformRepo, 
            IMapper mapper,
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient
            )
        {
            this.platformRepo = platformRepo;
            this.mapper = mapper;
            this.commandDataClient = commandDataClient;
            this.messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("---> Get All Platforms");
            var platforms = platformRepo.GetAll();
            return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }
         
        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            Console.WriteLine("---> Get Platform By Id");
            var platform = platformRepo.GetById(id);
            if (platform == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<PlatformReadDto>(platform));
        }
        
        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            Console.WriteLine("---> Create Platform");
            var platform = mapper.Map<Platform>(platformCreateDto);
            platformRepo.Create(platform);
            platformRepo.SaveChanges();
            var platformReadDto = mapper.Map<PlatformReadDto>(platform);

            // Send Sync Message
            try
            {
                await commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not send platform to command service sync: {ex}");
            }
            
            // Send Async Message
            try
            {
                var platformPublishedDto = mapper.Map<PlatformPublishedDto>(platformReadDto);
                platformPublishedDto.Event = "Platform_Published";
                messageBusClient.PublishNewPlatform(platformPublishedDto);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"---> Could not send platform to command service async: {ex}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
        }
    }
}
