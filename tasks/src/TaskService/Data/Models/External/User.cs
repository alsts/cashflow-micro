using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cashflow.Common.Data.Enums;
using Cashflow.Common.Data.Models;

namespace TaskService.Data.Models
{
    public class User : BaseEntity
    {
        [Required] public string Email { get; set; }
        [Required] public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string RefreshToken { get; set; }
        public Genders Gender { get; set; }
        public int RoleId { get; set; }
        public bool IsBanned { get; set; } = false;
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
