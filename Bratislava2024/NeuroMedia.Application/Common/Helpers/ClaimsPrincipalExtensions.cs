using System.Security.Claims;

using Microsoft.IdentityModel.JsonWebTokens;

namespace NeuroMedia.Application.Common.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public const string IdTokenHeader = "neuromediaidtoken"; // all small caps

        private const string PreferredUserName = "preferred_username";
        private const string Emails = "emails";
        private const string Oid1 = "oid";
        private const string Oid2 = "http://schemas.microsoft.com/identity/claims/objectidentifier";
        private const string Roles = "UserRoles";
        private const string Institution = "UserInstitution";

        public static string GetUserName(this ClaimsPrincipal claims)
        {
            var value = claims.FindFirst(JwtRegisteredClaimNames.Name)?.Value;

            return string.IsNullOrEmpty(value) ? claims.FindFirst(JwtRegisteredClaimNames.UniqueName).Value ?? string.Empty : value;
        }

        public static string GetEmail(this ClaimsPrincipal claims)
        {
            var value = claims.FindFirst(PreferredUserName)?.Value;

            if (string.IsNullOrEmpty(value))
            {
                value = claims.FindFirst(Emails)?.Value;
            }

            return string.IsNullOrEmpty(value) ? string.Empty : value;
        }

        public static string GetUserId(this ClaimsPrincipal claims)
        {
            var value = claims.FindFirst(Oid1)?.Value;

            if (string.IsNullOrEmpty(value))
            {
                value = claims.FindFirst(Oid2)?.Value;
            }

            return string.IsNullOrEmpty(value) ? string.Empty : value;
        }

        public static string GetTenantId(this ClaimsPrincipal claims)
        {
            var value = claims.FindFirst(Institution)?.Value;

            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            return claims.GetUserId();
        }

        public static string GetRoles(this ClaimsPrincipal claims)
        {
            var value = claims.FindFirst(Roles)?.Value;

            return string.IsNullOrEmpty(value) ? string.Empty : value;
        }

        public static bool IsInRoleCustomImplementation(this ClaimsPrincipal? claims, string role)
        {
            if (claims == null)
            {
                return false;
            }

            var identities = claims.Identities.ToList();

            if (identities != null)
            {
                for (var i = 0; i < identities.Count; i++)
                {
                    if (identities[i] != null)
                    {
                        if (identities[i].HasClaim(identities[i].RoleClaimType, role))
                        {
                            return true;
                        }
                    }
                }
            }

            var value = claims.FindFirst(Roles)?.Value;

            if (!string.IsNullOrEmpty(value))
            {
                var roles = value.Split(',');

                if (roles.Contains(role))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
