using Microsoft.AspNetCore.Authorization;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.API.Policies
{
    internal class CustomRoleRequirement(Roles customRoles) : IAuthorizationRequirement
    {
        public Roles CustomRoles { get; } = customRoles;
    }
}
