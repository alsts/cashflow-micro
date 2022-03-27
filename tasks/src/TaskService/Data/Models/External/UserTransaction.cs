using Cashflow.Common.Data.Models;

namespace TaskService.Data.Models.External
{
    public class UserTransaction : ExternalEntity
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int TransactionStatus { get; set; }
        public int TransactionType { get; set; }
        public int UserId { get; set; }
    }
}
