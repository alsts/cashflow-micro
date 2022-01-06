namespace AccountService.Util
{

    public class LoggedInUserDataHolder
    {
        // setters as methods just to make it more verbose
        public int UserID { get; private set; }
        public int RoleID { get; private set; }
        public string IPAddress { get; private set; }

        public void SetUserID(int userID)
        {
            UserID = userID;
        }

        public void SetRoleID(int roleID)
        {
            RoleID = roleID;
        }

        public void SetIPAddress(string ipAddress)
        {
            IPAddress = ipAddress;
        }
    }
}
