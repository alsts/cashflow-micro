using System;

namespace TaskService.Dtos.Promotion
{
    public class TaskReadDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal RewardPrice { get; set; }
    }
}
