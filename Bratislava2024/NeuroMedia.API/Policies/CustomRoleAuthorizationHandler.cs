using Microsoft.AspNetCore.Authorization;
using NeuroMedia.Domain.Enums;
using NeuroMedia.Application.Common.Helpers;
using NeuroMedia.Application.Logging;
using NeuroMedia.Application.Interfaces.Services;
using System.Security.Claims;

namespace NeuroMedia.API.Policies
{
    // This class contains logic for determining whether CustomRoleRequirement in authorization policies are satisfied or not
    internal class CustomRoleAuthorizationHandler(IUserInfoService userInfoService, IConfiguration configuration, ILogger<CustomRoleAuthorizationHandler> logger) : AuthorizationHandler<CustomRoleRequirement>
    {
        // Check whether a given CustomRoleRequirement is satisfied or not for a particular context
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRoleRequirement requirement)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(requirement);

            return HandleRequirementInternalAsync(context, requirement);
        }

        private async Task HandleRequirementInternalAsync(AuthorizationHandlerContext context, CustomRoleRequirement requirement)
        {
            var httpContext = context.Resource as DefaultHttpContext;
            var requestHeaders = httpContext?.Request.Headers;
            var cancellationToken = httpContext?.RequestAborted ?? CancellationToken.None;
            var useIdToken = bool.TrueString == configuration["Token:UseIdToken"];

            // Return if no token in header exists

            var token = useIdToken
                        ? (requestHeaders?.FirstOrDefault(x => x.Key == ClaimsPrincipalExtensions.IdTokenHeader))?.Value
                        : requestHeaders?.Authorization;

            if (token is null)
            {
                return;
            }

            token = token.Value.ToString().Replace("Bearer", string.Empty).Trim();

            var jst = useIdToken
                        ? await JwtValidationHelper.ValidateToken($"{token}", configuration, logger)
                        : JwtValidationHelper.ReadToken($"{token}", logger); // reading should be enough for access token, validation is done inside [Authorize]

            if (jst is null)
            {
                context.Fail(new AuthorizationFailureReason(this, "JWT token is invalid"));
                return;
            }

            var identity = useIdToken
                            ? new ClaimsPrincipal(new ClaimsIdentity(jst.Claims))
                            : httpContext?.User;

            var jwtRoles = RolesHelper.GetEnumRoles(identity!.GetRoles());

            if ((requirement.CustomRoles & jwtRoles) > Roles.None)
            {
                if (identity != null)
                {
                    await userInfoService.SetUserInfoAsync(identity, cancellationToken);
                }

                // Requirement satisfied
                context.Succeed(requirement);
            }
        }
    }

}
