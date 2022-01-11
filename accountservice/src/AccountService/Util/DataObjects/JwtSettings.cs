using System;

namespace AccountService.Util
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set;}
        public string Audience { get; set; }
        public int MinutesToExpiration { get; set; }

        public TimeSpan Expire => TimeSpan.FromMinutes(MinutesToExpiration);
    }
}
