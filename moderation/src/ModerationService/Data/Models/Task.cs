using System;
using Cashflow.Common.Data.Models;

namespace ModerationService.Data.Models
{
    public class Task : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int TaskStatus { get; set; }
        public DateTime ApprovedAt { get; set; }
    }
}
