using System;
using System.ComponentModel.DataAnnotations;

namespace CashFlow.Database.Models
{
    public class TaskTransaction
    {
        public DateTime CreatedAt { get; set; }
        public int CreatedByUserID { get; set; }
        public string CreateDescription { get; set; }
        [Key]
        public int ID { get; set; }
        public int TaskID { get; set; }
        public decimal Amount { get; set; }
        public int? ToUserID { get; set; }
    }
}