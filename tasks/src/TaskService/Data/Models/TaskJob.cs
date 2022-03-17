using Cashflow.Common.Data.Enums;
using Cashflow.Common.Data.Models;

namespace TaskService.Data.Models
{
    public class TaskJob : BaseEntity
    {
        public decimal RewardPrice { get; set; }
        public TaskJobStatus TaskJobStatus { get; set; }

        public int TaskId { get; set; }
        public Task Task { get; set; }
        
        public int? ReportId { get; set; }
        public Report? Report { get; set; }
    }
}
