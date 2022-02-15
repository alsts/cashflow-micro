using System;
using System.ComponentModel.DataAnnotations;

namespace Cashflow.Common.Data.Models
{
    public class BaseEntity
    {
        [Key] public int Id { get; set; }
        public string PublicId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string? LastUpdatedByUserId { get; set; }
        // Used for events:
        public int Version { get; set; }
    }
}
