using Cashflow.Common.Data.Enums;
using Cashflow.Common.Data.Models;

namespace MoneyService.Data.Models.External
{
    public class Task : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus TaskStatus { get; set; }
        public decimal RewardPrice { get; set; }
    }
}
