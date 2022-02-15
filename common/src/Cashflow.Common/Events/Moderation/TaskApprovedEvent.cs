using System;

namespace Cashflow.Common.Events.Moderation
{
    public class TaskApprovedEvent
    {
        public string TaskId { get; set; }
        public DateTime ApprovedAt { get; set; }
    }
}
