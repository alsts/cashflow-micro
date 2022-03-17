using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cashflow.Common.Data.Enums;
using Cashflow.Common.Data.Models;

namespace TaskService.Data.Models
{
    public class Report : BaseEntity
    {
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
        
        public int TaskJobId { get; set; }
        public TaskJob TaskJob { get; set; }
        
        public ICollection<ReportRevision> ReportRevisions { get; set; } = new List<ReportRevision>();
    }
}
