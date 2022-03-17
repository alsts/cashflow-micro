using System;

namespace MoneyService.Dtos
{
    public class TaskTransactionReadDto
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int TransactionStatus { get; set; }
        public int TaskId { get; set; }
    }
}
