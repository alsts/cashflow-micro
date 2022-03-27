using Cashflow.Common.Data.Models;

namespace TaskService.Data.Models.External
{
    public class TaskTransaction: ExternalEntity
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int TransactionStatus { get; set; }
        public string TaskId { get; set; }
    }
}
