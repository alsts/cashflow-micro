
using System;

namespace Cashflow.Common.Events.Tasks
{
    public class TaskUpdatedEvent : GenericEventProps
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int TaskStatus { get; set; }
        // Generic:
        public string PublicId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByUserID { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedByUserID { get; set; }
        public int Version { get; set; }
    }
}
