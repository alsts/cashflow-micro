using System;

namespace ModerationService.Dtos
{
    public class UserBanDto
    {
        public string UserId { get; set; }
        public DateTime BannedAt { get; set; } 
    }
}
