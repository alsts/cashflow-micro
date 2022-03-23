using System;
using System.Collections.Generic;
using Cashflow.Common.Data.Enums;
using Cashflow.Common.Data.Models;

namespace TaskService.Data.Models.External
{
    public class User : ExternalEntity
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Genders Gender { get; set; }
        public int RoleId { get; set; }
        public string RefreshToken { get; set; }
        // Moderation:
        public bool IsActive { get; set; }
        public int WarningsCount { get; set; }
        public DateTime? BannedAt { get; set; }
        // Relations:
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
