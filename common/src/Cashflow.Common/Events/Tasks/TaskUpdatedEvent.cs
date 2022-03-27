
using System;

namespace Cashflow.Common.Events.Tasks
{
    public class TaskUpdatedEvent
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal RewardPrice { get; set; }
        public int TaskStatus { get; set; }
        // Generic:
        public string PublicId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedByUserId { get; set; }
        public int Version { get; set; }
    }
}
