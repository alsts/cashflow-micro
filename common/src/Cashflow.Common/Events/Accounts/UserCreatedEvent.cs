
using System;

namespace Cashflow.Common.Events.Accounts
{
    public class UserCreatedEvent : GenericEventProps
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string RefreshToken { get; set; }
        public int Gender { get; set; }
        public int RoleId { get; set; } 
        // Generic:
        public string PublicId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByUserID { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedByUserID { get; set; }
        public int Version { get; set; }
    }
}
