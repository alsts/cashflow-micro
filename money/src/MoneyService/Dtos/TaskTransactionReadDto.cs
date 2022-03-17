using System;

namespace MoneyService.Dtos
{
    public class UserTransactionReadDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
