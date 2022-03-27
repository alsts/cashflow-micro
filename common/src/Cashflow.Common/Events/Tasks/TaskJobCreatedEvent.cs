
using System;

namespace Cashflow.Common.Events.Tasks
{
    public class TaskJobCreatedEvent 
    {
        public string TaskId { get; set; }
        public int TaskJobStatus { get; set; }
        // Generic:
        public string PublicId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedByUserId { get; set; }
        public int Version { get; set; }
    }
}
