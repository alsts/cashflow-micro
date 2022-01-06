using System;
using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IMapper mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            this.scopeFactory = scopeFactory;
            this.mapper = mapper;
        }
        
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                case EventType.Undetermined:
                    break;
            }
        }

        private void AddPlatform(string platformPublishedMessage)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                // no services can be registered with shorter lifetime services:
                // repository lives per each request - scoped!
                var commandRepo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

                try
                {
                    var platform = mapper.Map<Platform>(platformPublishedDto);
                    if (commandRepo.ExternalPlatformExist(platform.ExternalId))
                    {
                        Console.WriteLine($"---> Platform already exist");
                    }
                    
                    commandRepo.CreatePlatform(platform);
                    commandRepo.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"---> Could not add Platform to DB {e}");
                    throw;
                }
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("---> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            switch (eventType?.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("---> Platform Published Event");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("---> Undetermined Event");
                    return EventType.Undetermined;
            }
        }
    }
    
    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}
