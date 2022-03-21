using System;
using Cashflow.Common.Data.Models;

namespace ModerationService.Data.Models
{
    public class UserBan : BaseEntity
    {
        public string UserId { get; set; }
        public DateTime UserBannedAt { get; set; } 
    }
}
