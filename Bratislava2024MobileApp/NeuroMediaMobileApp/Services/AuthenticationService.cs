using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Identity.Client;

using NeuroMediaMobileApp.View;
using NeuroMediaMobileApp.Services.Interfaces;
using NeuroMediaMobileApp.Helpers;

namespace NeuroMediaMobileApp.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly string[] privilegedRoles = ["SystemManager", "Admin", "Doctor", "Nurse"];
        private readonly string patientRole = "Patient";
        private readonly ISessionService _sessionService;
        private readonly IPublicClientApplication _identityClient;

        public AuthenticationService(ISessionService sessionService)
        {
            _sessionService = sessionService;
#if ANDROID
            _identityClient = PublicClientApplicationBuilder
                    .Create(EntraConfig.ClientId)
                    .WithAuthority(AzureCloudInstance.AzurePublic, EntraConfig.TenantId)
                    .WithRedirectUri($"msal{EntraConfig.ClientId}://auth")
                    .WithParentActivityOrWindow(() => Platform.CurrentActivity)
                    .Build();
#elif IOS
            _identityClient = PublicClientApplicationBuilder
                .Create(EntraConfig.ClientId)
                .WithAuthority(AzureCloudInstance.AzurePublic, EntraConfig.TenantId)
                .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
                .WithRedirectUri($"msal{EntraConfig.ClientId}://auth")
                .Build();
#else
            _identityClient = PublicClientApplicationBuilder
                    .Create(EntraConfig.ClientId)
                    .WithAuthority(AzureCloudInstance.AzurePublic, EntraConfig.TenantId)
                    //.WithRedirectUri("https://login.microsoftonline.com/common/oauth2/nativeclient")
                    .WithRedirectUri("http://localhost:1234")
                    .Build();
#endif
        }
        public async Task CheckIfLoggedIn()
        {
            var accounts = await _identityClient.GetAccountsAsync();

            try
            {
                var result = await _identityClient
                    .AcquireTokenSilent(EntraConfig.Scopes, accounts.FirstOrDefault())
                    .ExecuteAsync();

                if (result != null)
                {
                    Console.WriteLine("Successfully logged in silently.");

                    var token = new AuthenticationToken
                    (
                        DisplayName: result.Account?.Username ?? "",
                        ExpiresOn: result.ExpiresOn,
                        AccessToken: result.AccessToken,
                        IdToken: result.IdToken,
                        UserId: result.Account?.Username ?? ""
                    );

                    _sessionService.CreateSession(
                        JwtHelper.GetName(token.AccessToken),
                        JwtHelper.GetEmail(token.AccessToken),
                        JwtHelper.GetRole(token.IdToken),
                        JwtHelper.GetOid(token.AccessToken),
                        JwtHelper.GetTenantId(token.AccessToken),
                        token.AccessToken,
                        token.IdToken);
                }
            }
            catch (MsalUiRequiredException)
            {
                Console.WriteLine("Silent login not possible; user interaction is required.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSAL Silent Error: {ex.Message}");
            }
        }


        public async Task Login()
        {
            var accounts = await _identityClient.GetAccountsAsync();
            AuthenticationResult? result = null;
            var tryInteractiveLogin = false;

            try
            {
                result = await _identityClient
                    .AcquireTokenSilent(EntraConfig.Scopes, accounts.FirstOrDefault())
                    .ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                tryInteractiveLogin = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSAL Silent Error: {ex.Message}");
                throw;
            }

            if (tryInteractiveLogin)
            {
                try
                {
                    result = await _identityClient
                        .AcquireTokenInteractive(EntraConfig.Scopes)
                        .ExecuteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"MSAL Interactive Error: {ex.Message}");
                    throw;
                }
            }

            var token = new AuthenticationToken
            (
                DisplayName: result?.Account?.Username ?? "",
                ExpiresOn: result?.ExpiresOn ?? DateTimeOffset.MinValue,
                AccessToken: result?.AccessToken ?? "",
                IdToken: result?.IdToken ?? "",
                UserId: result?.Account?.Username ?? ""
            );

            _sessionService.CreateSession(
                    JwtHelper.GetName(token.AccessToken),
                    JwtHelper.GetEmail(token.AccessToken),
                    JwtHelper.GetRole(token.IdToken),
                    JwtHelper.GetOid(token.AccessToken),
                    JwtHelper.GetTenantId(token.AccessToken),
                    token.AccessToken,
                    token.IdToken);

            return;
        }

        public async Task Logout()
        {
            if (_identityClient == null)
            {
                await Shell.Current.GoToAsync("///MainPage");
                return;
            }
            try
            {
                var accounts = await _identityClient.GetAccountsAsync();
                if (accounts.Any())
                {
                    foreach (var account in accounts)
                    {
                        await _identityClient.RemoveAsync(account);
                    }
                }

                _sessionService.ClearSession();
                var builder = new UriBuilder("https://login.microsoftonline.com/common/oauth2/v2.0/logout");
                await Browser.OpenAsync(builder.Uri, BrowserLaunchMode.SystemPreferred);
                await Shell.Current.GoToAsync("///MainPage");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logout failed: {ex.Message}");
            }
        }

        public bool IsPrivileged(string userRoles)
        {
            var roles = userRoles?.Split(',') ?? [];
            foreach (var role in roles)
            {
                if (privilegedRoles.Contains(role))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsPatient(string userRoles)
        {
            var roles = userRoles?.Split(',') ?? [];
            foreach (var role in roles)
            {
                if (role == patientRole)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
