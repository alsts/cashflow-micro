using System.Linq;
using System.Text;
using Cashflow.Common.Data.DataObjects;
using Cashflow.Common.Utils.HealthChecks;
using DefaultNamespace;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Cashflow.Common.Utils
{
    public static class Configuration
    {
        public static void ConfigureLogs(ILoggingBuilder logBuilder)
        {
            logBuilder.ClearProviders(); // removes all providers from LoggerFactory
            logBuilder.AddConsole();  
            logBuilder.AddTraceSource("Information, ActivityTracing"); // Add Trace listener provider
            logBuilder.AddSimpleConsole(options =>
            {
                options.IncludeScopes = false;
                options.SingleLine = true;
                options.TimestampFormat = "[MM/dd/yyyy HH:mm:ss] ";
            });
        }

        public static void UseCustomHealthChecks(this IEndpointRouteBuilder endpoints, string url)
        {
            endpoints.MapHealthChecks(url, new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new HealthCheckResponse
                    {
                        Status = report.Status.ToString(),
                        HealthChecks = report.Entries.Select(x => new IndividualHealthCheckResponse
                        {
                            Component = x.Key,
                            Status = x.Value.Status.ToString(),
                            Description = x.Value.Description
                        }),
                        HealthCheckDuration = report.TotalDuration
                    };
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }
            });
        }

        public static void AddJwtCookiesAuthentication(this IServiceCollection services, JwtSettings jwtSettings )
        {
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
        }
    }
}
