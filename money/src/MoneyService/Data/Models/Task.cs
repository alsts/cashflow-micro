using Cashflow.Common.Data.Models;

namespace MoneyService.Data.Models
{
    public class Task : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
