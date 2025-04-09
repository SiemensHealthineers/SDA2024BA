using Microsoft.AspNetCore.Authorization;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.API.Policies
{
    // This attribute derives from the [Authorize] attribute, adding
    // the ability for a user to specify a 'customRoles' parameter. Since authorization
    // policies are looked up from the policy provider only by string, this
    // authorization attribute creates its policy name based on a constant prefix
    // and the user-supplied Custom role parameter. A custom authorization policy provider
    // (`CustomRolePolicyProvider`) can then produce an authorization policy with
    // the necessary requirements based on this policy name.
    internal class CustomRoleAuthorizeAttribute : AuthorizeAttribute
    {
        public CustomRoleAuthorizeAttribute(Roles customRoles)
        {
            Policy = $"{CustomRolePolicyProvider.PolicyPrefix}{customRoles}";
        }
    }

}
