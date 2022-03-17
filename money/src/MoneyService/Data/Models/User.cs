using Cashflow.Common.Data.Models;

namespace MoneyService.Data.Models
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string RefreshToken { get; set; }
        public int RoleId { get; set; }
        public bool IsBanned { get; set; } = false;
    }
}
