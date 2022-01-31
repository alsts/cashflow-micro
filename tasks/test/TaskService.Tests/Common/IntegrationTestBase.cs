using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using TaskService.Data;
using TaskService.Data.Models;
using TaskService.Dtos;
using TaskService.Util.Enums;
using TaskService.Util.Jwt;
using Xunit;
using Task = System.Threading.Tasks.Task;
using TaskEntity = TaskService.Data.Models.Task;

namespace TaskService.Tests.Common
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
        protected readonly JwtTokenCreator JwtTokenCreator;
        public readonly IConfiguration Configuration;

        public IntegrationTestBase()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder
                        .ConfigureServices(services => { })
                        .UseEnvironment("Testing");
                });

            TestClient = appFactory.CreateClient(new WebApplicationFactoryClientOptions { HandleCookies = true });
            Configuration = appFactory.Services.CreateScope().ServiceProvider.GetRequiredService<IConfiguration>();
            DbContext = appFactory.Services.CreateScope().ServiceProvider.GetRequiredService<TDbContext>();
            JwtTokenCreator = appFactory.Services.CreateScope().ServiceProvider.GetRequiredService<JwtTokenCreator>();
            Fixture = new Fixture();

            SeedDB();
        }

        protected void AuthorizeRequestWithUser(HttpRequestMessage message, User user)
        {
            var token = JwtTokenCreator.GenerateForUser(user);
            message.Headers.Add("Cookie", $"X-Access-Token={token};");
        }

        protected void Arrange(Action<TDbContext> arrangeFunc)
        {
            arrangeFunc(DbContext);
            DbContext.SaveChanges();
        }

        private void SeedDB()
        {
            // RECREATE DB:
            // DbContext.Database.EnsureCreated();
        }

        public async void Dispose()
        {
            // reset DB state every test:
            DbContext.Tasks.RemoveRange(DbContext.Tasks);
            DbContext.Users.RemoveRange(DbContext.Users);
            await DbContext.SaveChangesAsync();

            // DELETE DB:
            // this.DbContext.Database.EnsureDeleted();
        }

        protected User CreateUser(String username, Roles role = Roles.User)
        {
            return new User
            {
                Email = $"{username}@cashflow.com",
                UserName = username,
                Firstname = username,
                Lastname = username,
                PublicId = Guid.NewGuid().ToString(),
                RefreshToken = Guid.NewGuid().ToString(),
                Gender = Genders.Male,
                RoleId = (int)role
            };
        }

        protected TaskEntity CreateTask(String title, int userId = 0)
        {
            return new TaskEntity
            {
                Title = title,
                PublicId = Guid.NewGuid().ToString(),
                Description = $"Description - {title}",
                UserId = userId
            };
        }
    }
}
