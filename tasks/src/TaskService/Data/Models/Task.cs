using System;
using System.ComponentModel.DataAnnotations;

namespace TaskService.Data.Models
{
    public class Task : BaseEntity
    {
        [Key] [Required] public int Id { get; set; }
        [Required] public string Title { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string PublicId { get; set; }
        [Required] public int UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
