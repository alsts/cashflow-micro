using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskService.Util.Enums;

namespace TaskService.Data.Models
{
    public class User : BaseEntity
    {
        [Key] [Required] public int Id { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string UserName { get; set; }
        [Required] public string PublicId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string RefreshToken { get; set; }
        public Genders Gender { get; set; }
        public int RoleId { get; set; }
        
        public bool IsBanned { get; set; } = false;
        
        public ICollection<Task> Tasks { get; set; } = new List<Task>();

    }
}
