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
            Roles = RoleConstants.Admin;
        }
    }
}
