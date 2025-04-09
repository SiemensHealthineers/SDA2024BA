using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using NeuroMedia.Application.Logging;

namespace NeuroMedia.Application.Common.Helpers
{
    public static class JwtValidationHelper
    {
        public static JwtSecurityToken? ReadToken(string token, ILogger? logger, LogRow? logRow = null)
        {
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var securityToken = handler.ReadJwtToken(token);

                return securityToken;
            }
            catch (Exception ex)
            {
                var newLogRow = new LogRow(logRow).UpdateCallerProperties();
                newLogRow.Message = $"{nameof(ReadToken)} exception";

                if (logger != null)
                {
                    logger!.LogError(new EventId(), ex, newLogRow.Message);
                }

                return null;
            }
        }

        public static async Task<JwtSecurityToken?> ValidateToken(string token, IConfiguration configuration, ILogger logger,
            bool validateLifeTime = true, LogRow? logRow = null, JwksData? jwksData = null)
        {
            try
            {
                var kid = GetKidFromTokenHeader(token);
                jwksData ??= await JwksHelper.GetKidDataPair(kid, configuration, logger, logRow);

                var rsa = new RSACryptoServiceProvider(2048);
                rsa.ImportParameters(new RSAParameters
                {
                    Modulus = Base64Helper.FromBase64Url(jwksData.Modulus),
                    Exponent = Base64Helper.FromBase64Url(jwksData.Exponent)
                });

                var validationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                    ValidAudiences = configuration["Token:Audiences"]!.Trim().Split(","),
                    ValidateAudience = true,
                    ValidIssuers = [configuration["Token:Issuer"], configuration["Token:IssuerMobile"]],
                    ValidateIssuer = true,
                    ValidateLifetime = validateLifeTime,
                    IssuerSigningKey = new RsaSecurityKey(rsa)
                };

                var handler = new JwtSecurityTokenHandler();
                var tokenValidationResult = await handler.ValidateTokenAsync(token, validationParameters);

                if (tokenValidationResult.IsValid)
                {
                    return tokenValidationResult
                        .SecurityToken as JwtSecurityToken;
                }

                var newLogRow = new LogRow(logRow).UpdateCallerProperties();
                newLogRow.Message = "Invalid token";
                logger.LogError(new EventId(), null, newLogRow.Message);

                return null;
            }
            catch (Exception ex)
            {
                var newLogRow = new LogRow(logRow).UpdateCallerProperties();
                newLogRow.Message = $"{nameof(ValidateToken)} exception";
                logger.LogError(new EventId(), ex, newLogRow.Message);

                return null;
            }
        }

        private static string GetKidFromTokenHeader(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var securityToken = handler.ReadJwtToken(token);

                if (securityToken != null)
                {
                    return securityToken.Header
                        .First(x => "kid".Equals(x.Key, StringComparison.Ordinal))
                        .Value?.ToString() ?? string.Empty;
                }
            }
            catch
            {
                //nothing to do
            }

            return string.Empty;
        }
    }
}
