
using System;

namespace Cashflow.Common.Events.Money
{
    public class TaskTransactionCreatedEvent
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int TransactionStatus { get; set; }
        public string TaskId { get; set; }
        // Generic:
        public string PublicId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedByUserId { get; set; }
        public int Version { get; set; }
    }
}
