using System;

namespace Cashflow.Common.Events.Moderation
{
    public class TaskChangesRequestedEvent
    {
        public string TaskId { get; set; }
        public string ChangesMessage { get; set; }
        public DateTime ChangesRequestedAt { get; set; } 
    }
}
