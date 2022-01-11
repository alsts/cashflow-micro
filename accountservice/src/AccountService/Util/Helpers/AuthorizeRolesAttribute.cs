using System;
using System.Linq;
using AccountService.Util.Enums;
using Microsoft.AspNetCore.Authorization;

namespace AccountService.Util.Helpers
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
