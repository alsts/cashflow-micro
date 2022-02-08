using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cashflow.Common.Data.DataObjects;
using Microsoft.IdentityModel.Tokens;
using TaskService.Data.Models;

namespace TaskService.Tests.Common
{
    public class FakeJwtTokenCreator
    {
        private readonly JwtSettings settings;

        public FakeJwtTokenCreator(JwtSettings settings)
        {
            this.settings = settings;
        }
        public string GenerateForUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                new Claim(ClaimTypes.UserData, user.PublicId),
                new Claim(ClaimTypes.Authentication, user.RefreshToken)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                settings.Issuer,
                settings.Audience,
                claims,
                expires: DateTime.UtcNow + settings.Expire,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
