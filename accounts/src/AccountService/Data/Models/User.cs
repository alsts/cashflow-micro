using System;
using System.ComponentModel.DataAnnotations;
using AccountService.Models;
using Cashflow.Common.Data.Enums;

namespace AccountService.Data.Models
{
    public class User : BaseEntity
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
        public bool IsBanned { get; set; } = false;
    }
}
