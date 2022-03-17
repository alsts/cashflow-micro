using System;
using Cashflow.Common.Data.Models;

namespace ModerationService.Data.Models
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public int WarningsCount { get; set; }
        public string RefreshToken { get; set; }
        public int RoleId { get; set; }
        public DateTime UserBlockedAt { get; set; } 
        public bool IsBanned { get; set; } = false;
    }
}
