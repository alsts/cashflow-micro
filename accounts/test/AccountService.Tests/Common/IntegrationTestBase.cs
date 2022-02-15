using System;
using System.Linq;
using System.Net.Http;
using AccountService.Data;
using AccountService.Data.Models;
using AccountService.Dtos;
using AutoFixture;
using Cashflow.Common.Data.Enums;
using Cashflow.Common.Utils.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace AccountService.Tests.Common
{
    /*
     * Use Assembly to speed up performance
     * Each Integration test is isolated (new Instance of TestClient)
     * The database state is being reset after each test finished
     * That way we can make sure that data is always tested properly
     * and there is no side effects from historical data
     */
    [Collection("AssemblyFixture")]
    public class IntegrationTestBase<TDbContext, TStartup> : IDisposable
        where TDbContext : AppDbContext
        where TStartup : class
    {
        protected TDbContext DbContext;
        protected readonly IFixture Fixture;
        protected readonly HttpClient TestClient;
        public readonly IPasswordHasher PasswordHasher;
        public readonly IConfiguration Configuration;

        public IntegrationTestBase()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder
                        .ConfigureServices(services => {})
                        .UseEnvironment("Testing");
                });
            
            TestClient = appFactory.CreateClient(new WebApplicationFactoryClientOptions {HandleCookies = true});
            Configuration = appFactory.Services.CreateScope().ServiceProvider.GetRequiredService<IConfiguration>();
            PasswordHasher = appFactory.Services.CreateScope().ServiceProvider.GetRequiredService<IPasswordHasher>();
            DbContext = appFactory.Services.CreateScope().ServiceProvider.GetRequiredService<TDbContext>();
            Fixture = new Fixture();
            
            SeedDB();
        }
        
        protected async Task AuthenticateAsync(String username)
        {
            // attach JWT cookies to TestClient
            await TestClient.PostAsJsonAsync(ApiRoutes.Account.SignIn, new UserSignInDto
            {
                Username = username,
                Password = "password"
            }); 
        }

        protected void Arrange(Action<TDbContext> arrangeFunc)
        {
            arrangeFunc(DbContext);
            DbContext.SaveChanges();
        }
        
        private void SeedDB()
        {
            if (!DbContext.Roles.Any())
            {
                DbContext.Roles.AddRange(
                    CreateRole("user", 1),
                    CreateRole("admin", 2),
                    CreateRole("superadmin", 3)
                ); 
            }

            // RECREATE DB:
            // DbContext.Database.EnsureCreated();
        }

        public async void Dispose()
        {
            // TestClient.DefaultRequestHeaders.Authorization = null;
         
            // reset DB state every test:
            DbContext.Users.RemoveRange(DbContext.Users);
            await DbContext.SaveChangesAsync();
            
            // DELETE DB:
            // this.DbContext.Database.EnsureDeleted();
        }

        protected Role CreateRole(String name, int id)
        {
            return new Role
            {
                Id = id,
                Name = name,
                Description = $"Basic {name}"
            };
        }
        
        protected User CreateUser(String username, Roles role = Roles.User)
        {
            return new User
            {

                Email = $"{username}@cashflow.com",
                UserName = username,
                Firstname =  username,
                Lastname = username,
                PublicId = Guid.NewGuid().ToString(),
                Password = PasswordHasher.Hash("password"),
                CreatedAt = DateTime.Now,
                IsActive = true,
                RefreshToken = null,
                Gender = Genders.Male,
                RoleId = (int) role
            };
        }
    }
}
