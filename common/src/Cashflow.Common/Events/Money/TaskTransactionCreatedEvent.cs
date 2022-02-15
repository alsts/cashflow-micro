
using System;

namespace Cashflow.Common.Events.Money
{
    public class TaskTransactionCreatedEvent
    {
        public float Amount { get; set; }
        public int TransactionStatus { get; set; }
        public string TransactionType { get; set; }
        // Generic:
        public string PublicId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByUserID { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedByUserID { get; set; }
        public int Version { get; set; }
    }
}
