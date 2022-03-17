using Cashflow.Common.Data.Models;

namespace MoneyService.Data.Models
{
    public class UserTransaction : BaseEntity
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int TransactionStatus { get; set; }
        public int TransactionType { get; set; }
        public int UserId { get; set; }
    }
}
