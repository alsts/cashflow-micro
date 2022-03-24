using System;

namespace TaskService.Dtos.Income
{
    public class IncomeTaskDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public int TaskStatus { get; set; }
        public int Version { get; set; }
        public decimal RewardPrice { get; set; }
        public decimal AvailableBalance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
