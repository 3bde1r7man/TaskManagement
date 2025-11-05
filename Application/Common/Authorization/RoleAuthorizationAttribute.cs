using Domain.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Application.Common.Authorization
{
    /// <summary>
    /// Authorization attribute for User role
    /// </summary>
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public AuthorizeUserAttribute()
        {
            // Set the authentication scheme explicitly
            AuthenticationSchemes = "Bearer";
            Roles = RoleConstants.User;
        }
    }

    /// <summary>
    /// Authorization attribute for Admin role
    /// </summary>
    public class AuthorizeAdminAttribute : AuthorizeAttribute
    {
        public AuthorizeAdminAttribute()
        {
            // Set the authentication scheme explicitly
            AuthenticationSchemes = "Bearer";
            Roles = RoleConstants.Admin;
        }
    }

    /// <summary>
    /// Authorization attribute for multiple roles
    /// </summary>
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles)
        {
            AuthenticationSchemes = "Bearer";
            Roles = string.Join(",", roles);
        }
    }
}