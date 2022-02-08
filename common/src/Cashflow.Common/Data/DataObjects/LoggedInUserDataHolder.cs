namespace Cashflow.Common.Data.DataObjects
{
    public class LoggedInUserDataHolder
    {
        public string UserID { get; set; }
        public int RoleID { get; set; }
        public string RefreshToken { get; set; }
        public string IPAddress { get; set; }
    }
}
