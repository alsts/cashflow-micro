using System;
using System.Collections.Generic;
using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

            var platforms = grpcClient.ReturnAllPlatforms();

            SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
        }

        private static void SeedData(ICommandRepo commandRepo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("---> Seeding data");

            foreach (var platform in platforms)
            {
                if (commandRepo.ExternalPlatformExist(platform.ExternalId)) continue;
                commandRepo.CreatePlatform(platform);
                commandRepo.SaveChanges();
            }
        }
    }
}
