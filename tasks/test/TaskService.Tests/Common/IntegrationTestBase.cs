using System;
using System.Net.Http;
using AutoFixture;
using AutoMapper;
using Cashflow.Common.Data.DataObjects;
using Cashflow.Common.Data.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskService.Data;
using TaskService.Data.Models;
using TaskService.Data.Models.External;
using TaskService.Events.Publishers.Interfaces;
using TaskService.Tests.Common.Stubs;
using Xunit;
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
        protected readonly FakeJwtTokenCreator FakeJwtTokenCreator;
        public readonly IConfiguration Configuration;
        public readonly IMapper Mapper;

        public IntegrationTestBase()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder
                        .ConfigureServices(services =>
                        {
                            // Add Mock of external services here:
                            services.AddScoped<IMessageBusPublisher, FakeMessageBusPublisher>();
                        })
                        .UseEnvironment("Testing");
                });

            TestClient = appFactory.CreateClient(new WebApplicationFactoryClientOptions { HandleCookies = true });
            Configuration = appFactory.Services.CreateScope().ServiceProvider.GetRequiredService<IConfiguration>();
            DbContext = appFactory.Services.CreateScope().ServiceProvider.GetRequiredService<TDbContext>();
            Mapper = appFactory.Services.CreateScope().ServiceProvider.GetRequiredService<IMapper>();

            var jwtSettings = appFactory.Services.CreateScope().ServiceProvider.GetRequiredService<JwtSettings>();
            
            FakeJwtTokenCreator = new FakeJwtTokenCreator(jwtSettings);
            Fixture = new Fixture();

            SeedDB();
        }

        protected void AuthorizeRequestWithUser(HttpRequestMessage message, User user)
        {
            var token = FakeJwtTokenCreator.GenerateForUser(user);
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
