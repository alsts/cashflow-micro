using System.Linq;
using Microsoft.AspNetCore.Authorization;
using TaskService.Util.Enums;

namespace TaskService.Util.Helpers
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params Roles[] allowedRoles)
        {
            var allowedRolesAsStrings = allowedRoles.Select(roleId => ((int) roleId).ToString());
            Roles = string.Join(",", allowedRolesAsStrings);
        }
    }
}
