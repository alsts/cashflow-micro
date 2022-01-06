using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AccountService.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AccountService.Middleware
{
    public class CustomJwtBearerEvents : JwtBearerEvents
    {
        private readonly LoggedInUserDataHolder loggedInUserDataHolder;

        public CustomJwtBearerEvents(LoggedInUserDataHolder loggedInUserDataHolder)
        {
            this.loggedInUserDataHolder = loggedInUserDataHolder;
        }
        
        public override async Task TokenValidated(TokenValidatedContext context)
        {
            var value = (context.SecurityToken as JwtSecurityToken)?.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;
            if (value != null)
            {
                loggedInUserDataHolder.SetUserID(int.Parse(
                    value
                ));

                loggedInUserDataHolder.SetRoleID(int.Parse(
                    (context.SecurityToken as JwtSecurityToken)?.Claims.FirstOrDefault(x => x.Type == "role")?.Value ?? string.Empty
                ));
            }

            var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ipAddress))
            {
                throw new Exception("Could not read IP address");
            }
            
            loggedInUserDataHolder.SetIPAddress(ipAddress);
        }
    }
}
