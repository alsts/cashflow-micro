using System.ComponentModel.DataAnnotations;
using Cashflow.Common.Data.Enums;
using Cashflow.Common.Data.Models;

namespace TaskService.Data.Models
{
    public class Task : BaseEntity
    {
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
        public TaskStatus TaskStatus { get; set; }
        public decimal RewardPrice { get; set; }
        
        // Comes from UserService:
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
