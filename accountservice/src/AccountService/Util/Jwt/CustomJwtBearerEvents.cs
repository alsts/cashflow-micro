using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountService.Util.DataObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AccountService.Util.Jwt
{
    public class CustomJwtBearerEvents : JwtBearerEvents
    {
        private readonly LoggedInUserDataHolder loggedInUserDataHolder;

        public CustomJwtBearerEvents(LoggedInUserDataHolder loggedInUserDataHolder)
        {
            this.loggedInUserDataHolder = loggedInUserDataHolder;
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
            var userIdClaimValue = (context.SecurityToken as JwtSecurityToken)?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var roleIdClaimValue = (context.SecurityToken as JwtSecurityToken)?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            if (userIdClaimValue != null && roleIdClaimValue != null)
            {
                loggedInUserDataHolder.UserID = int.Parse(userIdClaimValue);
                loggedInUserDataHolder.RoleID = int.Parse(roleIdClaimValue);
            }

            var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ipAddress))
            {
                throw new Exception("Could not read IP address");
            }
            loggedInUserDataHolder.IPAddress = ipAddress;
            
            return Task.CompletedTask;
        }
    }
}
