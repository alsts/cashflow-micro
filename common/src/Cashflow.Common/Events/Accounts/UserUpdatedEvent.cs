
using System;

namespace Cashflow.Common.Events.Accounts
{
    public class UserUpdatedEvent
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string RefreshToken { get; set; }
        public int Gender { get; set; }
        public int RoleId { get; set; } 
        // Moderation:
        public bool IsActive { get; set; }
        public int WarningsCount { get; set; }
        public DateTime? BannedAt { get; set; }
        // Generic:
        public string PublicId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedByUserId { get; set; }
        public int Version { get; set; }
    }
}
