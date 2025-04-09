using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.API.Policies
{
    internal class CustomRolePolicyProvider(IOptions<AuthorizationOptions> options) : IAuthorizationPolicyProvider
    {
        public const string PolicyPrefix = "CustomRole";

        // ASP.NET Core only uses one authorization policy provider, so if the custom implementation
        // doesn't handle all policies (including default policies, etc.) it should fall back to an
        // alternate provider.
        //
        // A default authorization policy provider (constructed with options from the
        // dependency injection container) is used if this custom provider isn't able to handle a given
        // policy name.
        //
        // If a custom policy provider is able to handle all expected policy names then, of course, this
        // fallback pattern is unnecessary.
        private DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; } = new DefaultAuthorizationPolicyProvider(options);

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return FallbackPolicyProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return FallbackPolicyProvider.GetFallbackPolicyAsync();
        }

        // Policies are looked up by string name, so expect 'parameters' (like customRole)
        // to be embedded in the policy names. This is abstracted away from developers
        // by the more strongly-typed attributes derived from AuthorizeAttribute
        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return FallbackPolicyProvider.GetPolicyAsync(policyName);
            }

            var customRoles = policyName[PolicyPrefix.Length..];

            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new CustomRoleRequirement(Enum.Parse<Roles>(customRoles)));

            return Task.FromResult(policy?.Build());

            // If the policy name doesn't match the format expected by this policy provider,
            // try the fallback provider. If no fallback provider is used, this would return
            // Task.FromResult<AuthorizationPolicy>(null) instead.
        }
    }
}
