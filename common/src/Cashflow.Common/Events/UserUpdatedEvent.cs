
namespace Cashflow.Common.Events
{
    public class UserUpdatedEvent
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PublicId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string RefreshToken { get; set; }
        public int Gender { get; set; }
        public int RoleId { get; set; } 
        public int Version { get; set; }
    }
}
