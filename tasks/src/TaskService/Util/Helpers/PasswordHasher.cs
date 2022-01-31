using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using TaskService.Util.Helpers.Interfaces;

namespace TaskService.Util.Helpers
{
    public class PasswordHasher : IPasswordHasher
    {
        public IConfiguration configuration { get; }
        
        public PasswordHasher(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        public string Hash(string password)
        {
            var saltBytes = Convert.FromBase64String(configuration["Authentication:PasswordSalt"]);
 
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 20000,
                numBytesRequested: 256 / 8));
        }
    }
}
