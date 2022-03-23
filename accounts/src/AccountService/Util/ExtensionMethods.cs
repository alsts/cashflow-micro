using AccountService.Data.Models;
using Microsoft.AspNetCore.Http;

namespace AccountService.Util
{
    namespace AccountService.Util.Helpers
    {
        public static class ExtensionMethods
        {
            public static User WithoutPassword(this User user) 
            {
                if (user == null) return null;

                user.Password = null;
                return user;
            }
        
            public static void AppendAuthCookie(this HttpResponse response, User user, string token) 
            {
                if (response == null) return;
                response.Cookies.Append("X-Access-Token", token, new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });
            }
        }
    }
}
