using Cashflow.Common.Data.Models;

namespace MoneyService.Data.Models
{
    public class TaskTransaction : BaseEntity
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int TransactionStatus { get; set; }
        public int TaskId { get; set; }
    }
}
