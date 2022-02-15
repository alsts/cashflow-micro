using System;
using System.Linq;
using AccountService.Data.Models;
using Cashflow.Common.Data.Enums;
using Cashflow.Common.Utils.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AccountService.Data
{
    public static class PrepDb
    {
        public static void Seed(IApplicationBuilder app, ILogger<Startup> logger, IWebHostEnvironment env)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var appDbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
            var passwordHasher = serviceScope.ServiceProvider.GetService<IPasswordHasher>();
            SeedData(appDbContext, passwordHasher, logger, env);
        }

        private static void SeedData(AppDbContext context, IPasswordHasher passwordHasher, ILogger<Startup> logger, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Testing")
            {
                return;
            }
            
            if (env.IsProduction())
            {
                logger.LogInformation("---> Applying migrations");
                try
                {
                    context.Database.EnsureCreated();
                    context.Database.Migrate();
                }
                catch (Exception e)
                {
                    logger.LogError("---> Migrations failed to apply");
                }
            }
            
            logger.LogInformation("---> Seeding data");
            
            if (!context.Roles.Any())
            {
                var roleUser = new Role
                {
                    Id = 1,
                    Name = "User",
                    Description = "Basic User"
                };

                var roleAdmin = new Role
                {
                    Id = 2,
                    Name = "Admin",
                    Description = "Admin User"
                };

                var roleSuperAdmin = new Role
                {
                    Id = 3,
                    Name = "SuperAdmin",
                    Description = "SuperAdmin User"
                };
                
                context.Roles.AddRange(roleUser, roleAdmin, roleSuperAdmin);
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var adminUser = new User
                {

                    Email = "admin@casflow.com",
                    UserName = "admin",
                    Firstname = "Admin",
                    Lastname = "Admin",
                    Password = passwordHasher.Hash("password"),
                    CreatedAt = new DateTime(),
                    IsActive = true,
                    PublicId = "cashflow-admin-user",
                    RefreshToken = null,
                    Gender = Genders.Male,
                    RoleId = (int) Roles.Admin
                };
                
                var superAdminUser = new User
                {

                    Email = "superadmin@casflow.com",
                    UserName = "superadmin",
                    Firstname = "Superadmin",
                    Lastname = "Superadmin",
                    Password = passwordHasher.Hash("password"),
                    CreatedAt = new DateTime(),
                    IsActive = true,
                    PublicId = "cashflow-superadmin-user",
                    RefreshToken = null,
                    Gender = Genders.Male,
                    RoleId = (int) Roles.SuperAdmin
                };

                context.Users.AddRange(superAdminUser, adminUser);
                context.SaveChanges();
            }
        }
    }
}
