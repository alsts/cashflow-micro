using System;
using System.ComponentModel.DataAnnotations;
using AccountService.Util.Enums;

namespace AccountService.Models
{
    public class User
    {
        [Key] [Required] public int Id { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string UserName { get; set; }
        [Required] public string PublicId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public string RefreshToken { get; set; }
        public Genders Gender { get; set; }
        public int RoleId { get; set; }
    }
}
