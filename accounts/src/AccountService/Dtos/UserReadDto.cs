using System;

namespace AccountService.Dtos
{
    public class UserReadDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
