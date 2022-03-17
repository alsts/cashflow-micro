using System;

namespace TaskService.Dtos
{
    public class UserPublishedDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Event { get; set; }
    }
}
