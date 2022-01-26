using System;
using System.Text;
using AccountService.Data;
using AccountService.Middlewares;
using AccountService.Services;
using AccountService.Services.interfaces;
using AccountService.Util;
using AccountService.Util.DataObjects;
using AccountService.Util.Helpers;
using AccountService.Util.Helpers.Interfaces;
using AccountService.Util.Jwt;
using AccountService.Util.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AccountService
{
    public class Startup
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<Startup> logger;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
            
            // startup logger:
            var loggerFactory = LoggerFactory.Create(Utils.ConfigureLogs);
            logger = loggerFactory.CreateLogger<Startup>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (env.IsProduction())
            {
                logger.LogInformation("---> Using MySql Db");
                var connectionString = Configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<AppDbContext>(opt => opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            }
            else
            {
                logger.LogInformation("---> Using inMem Db");
                services.AddDbContext<AppDbContext>(opt =>
                    opt.UseInMemoryDatabase("InMem"));
            }

            var jwtSettings = new JwtSettings();
            Configuration.Bind("JwtSettings", jwtSettings);

            services.AddSingleton(jwtSettings);
            services.AddTransient<JwtTokenCreator>();

            services.AddAuthentication(i =>
                {
                    i.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    i.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    i.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    i.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                        ClockSkew = jwtSettings.Expire
                    };
                    options.SaveToken = true;
                    options.EventsType = typeof(CustomJwtBearerEvents);
                })
                .AddCookie(options =>
                {
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.IsEssential = true;
                });

            services.AddTransient<IUserRepo, UserRepo>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddControllers();
            services.AddScoped<LoggedInUserDataHolder>();
            services.AddScoped<CustomJwtBearerEvents>();

            // configure DI for application services
            services.AddHttpContextAccessor();

            // register automapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "AccountService", Version = "v1" }); });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AccountService v1"));
            }
            
            // requests logging middleware
            app.UseMiddleware<RequestLoggingMiddleware>();

            // routing
            app.UseHttpsRedirection();
            app.UseRouting();
            
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            
            // authentication
            app.UseAuthentication();
            app.UseAuthorization();

            // global exception handler
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            // db seeder:
            PrepDb.Seed(app, logger, env);
        }
    }
}
