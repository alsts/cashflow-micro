using System.ComponentModel.DataAnnotations;
using Cashflow.Common.Data.Enums;
using Cashflow.Common.Data.Models;

namespace TaskService.Data.Models
{
    public class ReportRevision : BaseEntity
    {
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
        
        public int ReportId { get; set; }
        public Report Report { get; set; }
    }
}
