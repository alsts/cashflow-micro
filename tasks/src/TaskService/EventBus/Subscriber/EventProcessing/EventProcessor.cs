using System;
using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TaskService.Data.Models;
using TaskService.Data.Repos.Interfaces;
using TaskService.Dtos;

namespace TaskService.EventBus.Subscriber.EventProcessing
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
                case EventType.UserPublished:
                    AddUser(message);
                    break;
                case EventType.Undetermined:
                    break;
            }
        }

        private async void AddUser(string userPublishedMessage)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                // no services can be registered with shorter lifetime services:
                // repository lives per each request - scoped!
                var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepo>();

                var userPublishedDto = JsonSerializer.Deserialize<UserPublishedDto>(userPublishedMessage);

                try
                {
                    var user = await userRepo.GetByPublicId(userPublishedDto.Id);
                    
                    if (user != null)
                    {
                        Console.WriteLine($"---> User already exist");
                        return;
                    }

                    var newUser = new User
                    {
                        PublicId = userPublishedDto.Id,
                        Email = userPublishedDto.Email,
                        UserName = userPublishedDto.Username
                    };
                    
                    await userRepo.Save(newUser);
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
                case "User_Published":
                    Console.WriteLine("---> User Published Event");
                    return EventType.UserPublished;
                default:
                    Console.WriteLine("---> Undetermined Event");
                    return EventType.Undetermined;
            }
        }
    }
    
    enum EventType
    {
        UserPublished,
        Undetermined
    }
}
