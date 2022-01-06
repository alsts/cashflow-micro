using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformServices.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd
            );
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {

            if (isProd)
            {
                Console.WriteLine("---> Applying migrations");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"---> Migrations failed to apply {e.Message}");
                }
            }

            if (!context.Platforms.Any())
            {
                Console.WriteLine("---> Seeding data");
                context.Platforms.AddRange(
                    new Platform { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                    new Platform { Name = "SQL", Publisher = "Microsoft", Cost = "Free" },
                    new Platform { Name = "Kubernetes", Publisher = "Google", Cost = "Paid" }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("---> We already have data");
            }
        }
    }
}
