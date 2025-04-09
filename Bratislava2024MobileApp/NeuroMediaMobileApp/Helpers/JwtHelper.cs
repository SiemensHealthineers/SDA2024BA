using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroMediaMobileApp.Helpers
{
    public static class JwtHelper
    {
        public static string GetOid(string token)
        {
            return GetClaimValue(token, "oid");
        }

        public static string GetName(string token)
        {
            return GetClaimValue(token, "name");
        }

        public static string GetTenantId(string token)
        {
            return GetClaimValue(token, "tid");
        }
        public static string GetEmail(string token)
        {
            return GetClaimValue(token, "preferred_username");
        }
        public static string GetRole(string token)
        {
            return GetClaimValue(token, "UserRoles");
        }

        private static string GetClaimValue(string token, string claimType)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == claimType);
            return claim?.Value ?? string.Empty;
        }
    }
}
