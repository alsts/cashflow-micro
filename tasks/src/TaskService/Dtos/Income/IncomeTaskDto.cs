using System;

namespace TaskService.Dtos.Income
{
    public class IncomeTaskDto
    {
        public string PublicId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public int TaskStatus { get; set; }
        public bool IsActive { get; set; }
        
        public int Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
