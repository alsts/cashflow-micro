using System.Linq;
using Cashflow.Common.Data.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Cashflow.Common.Utils
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
