
using System;

namespace Cashflow.Common.Events.Tasks
{
    public class TaskJobUpdatedEvent : GenericEventProps
    {
        public string TaskId { get; set; }
        public int TaskJobStatus { get; set; }
        // Generic:
        public string PublicId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByUserID { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedByUserID { get; set; }
        public int Version { get; set; }
    }
}
