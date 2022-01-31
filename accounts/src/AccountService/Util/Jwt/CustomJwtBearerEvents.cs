using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountService.Util.DataObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AccountService.Util.Jwt
{
    public class CustomJwtBearerEvents : JwtBearerEvents
    {
        private readonly LoggedInUserDataHolder loggedInUserDataHolder;
        private readonly IWebHostEnvironment env;

        public CustomJwtBearerEvents(LoggedInUserDataHolder loggedInUserDataHolder, IWebHostEnvironment env)
        {
            this.loggedInUserDataHolder = loggedInUserDataHolder;
            this.env = env;
        }

        public override Task MessageReceived(MessageReceivedContext context)
        {
            if (context.Request.Cookies.ContainsKey("X-Access-Token"))
            {
                context.Token = context.Request.Cookies["X-Access-Token"];
            }
            return Task.CompletedTask;
        }

        public override Task TokenValidated(TokenValidatedContext context)
        {
            var userIdClaimValue = (context.SecurityToken as JwtSecurityToken)?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData)?.Value;
            var roleIdClaimValue = (context.SecurityToken as JwtSecurityToken)?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            var refreshTokenClaimValue = (context.SecurityToken as JwtSecurityToken)?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication)?.Value;
            
            if (userIdClaimValue != null && roleIdClaimValue != null && refreshTokenClaimValue != null)
            {
                loggedInUserDataHolder.UserID = userIdClaimValue;
                loggedInUserDataHolder.RoleID = int.Parse(roleIdClaimValue);
                loggedInUserDataHolder.RefreshToken = refreshTokenClaimValue;
            }

            // IP address can not be retrieved on integration tests or development environments:
            var ipAddress = env.IsProduction() ? context.HttpContext.Connection.RemoteIpAddress?.ToString() : IPAddress.Any.ToString();
            if (string.IsNullOrEmpty(ipAddress))
            {
                throw new Exception("Could not read IP address");
            }
            loggedInUserDataHolder.IPAddress = ipAddress;
            
            return Task.CompletedTask;
        }
    }
}
