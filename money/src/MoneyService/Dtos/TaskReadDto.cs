using System;

namespace MoneyService.Dtos
{
    public class TaskReadDto
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int TransactionStatus { get; set; }
        public int TransactionType { get; set; }
        public int UserId { get; set; }
    }
}
