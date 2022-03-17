
using System;

namespace Cashflow.Common.Events.Money
{
    public class TaskJobTransactionCreatedEvent : GenericEventProps
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int TransactionStatus { get; set; }
        public int TaskJobId { get; set; }
        // Generic:
        public string PublicId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByUserID { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedByUserID { get; set; }
        public int Version { get; set; }
    }
}
