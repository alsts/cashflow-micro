using System;

namespace Cashflow.Common.Events.Moderation
{
    public class UserBannedEvent
    {
        public string UserId { get; set; }
        public DateTime UserBannedAt { get; set; } 
    }
}
