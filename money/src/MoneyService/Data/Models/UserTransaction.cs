using System;
using System.ComponentModel.DataAnnotations;

namespace CashFlow.Database.Models
{
    public class UserTransaction
    {
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Do NOT rely on this to map user and transaction,
        /// as it might have been created by a system. User UserID instead
        /// </summary>
        public int CreatedByUserID { get; set; }
        public string CreateDescription { get; set; }
        [Key]
        public int ID { get; set; }

        public decimal Amount { get; set; }
        public int UserID { get; set; }
        public bool IsAdTransaction { get; set; }
        public string PaymentIntentID { get; set; }
        public bool IsDeposit { get; set; }
        public bool IsWithdrawal { get; set; }
    }
}