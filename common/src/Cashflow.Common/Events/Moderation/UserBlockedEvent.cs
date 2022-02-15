using System;

namespace Cashflow.Common.Events.Moderation
{
    public class UserBlockedEvent
    {
        public string UserId { get; set; }
        public DateTime UserBlockedAt { get; set; } 
    }
}
