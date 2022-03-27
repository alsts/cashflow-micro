namespace Cashflow.Common.Data.DataObjects
{
    public class LoggedInUserDataHolder
    {
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public string RefreshToken { get; set; }
        public string IPAddress { get; set; }
    }
}
