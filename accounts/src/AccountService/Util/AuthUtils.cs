using System.Text.RegularExpressions;

namespace AccountService.Util
{
    public static  class AuthUtils
    {
        public static bool IsPasswordValid(string password)
        {
            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$");
            return passwordRegex.IsMatch(password);
        } 
    }
}
