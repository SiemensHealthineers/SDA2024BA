using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;

using NeuroMedia.Application.Features.Users.Queries.GetAllUsers;
using NeuroMedia.WebUI.Services.Interfaces;

using static System.Net.WebRequestMethods;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;
using NeuroMedia.Application.Common.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;
using System.Security.Claims;

namespace NeuroMedia.WebUI.Services
{
    public class HttpClientService(HttpClient httpClient, IAccessTokenProvider tokenProvider, IJSRuntime jsRuntime, IConfiguration configuration) : IHttpClientService
    {
        public async Task<List<T>> GetListAsync<T>(string url, CancellationToken cancellationToken = default)
        {
            await SetTokens();

            var result = await httpClient.GetFromJsonAsync<List<T>>(url, cancellationToken);

            return result ?? [];
        }

        public async Task<T?> GetAsync<T>(string url, CancellationToken cancellationToken = default)
        {
            await SetTokens();

            var result = await httpClient.GetFromJsonAsync<T>(url, cancellationToken);

            return result;
        }

        public async Task<bool> PostAsync<T>(string url, T data, CancellationToken cancellationToken = default)
        {
            await SetTokens();

            var result = await httpClient.PostAsJsonAsync(url, data, cancellationToken);

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> PutAsync<T>(string url, T data, CancellationToken cancellationToken = default)
        {
            await SetTokens();

            var result = await httpClient.PutAsJsonAsync(url, data, cancellationToken);

            return result.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string url, CancellationToken cancellationToken = default)
        {
            await SetTokens();

            try
            {
                var result = await httpClient.DeleteAsync(url, cancellationToken);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> PutEmptyAsync(string url, CancellationToken cancellationToken = default)
        {
            await SetTokens();

            var result = await httpClient.PutAsync(url, null, cancellationToken);

            return result.IsSuccessStatusCode;
        }

        public async Task<HttpResponseMessage?> PutAsyncWithResponseMessage<T>(string url, T data, CancellationToken cancellationToken = default)
        {
            await SetTokens();

            var result = await httpClient.PutAsJsonAsync(url, data, cancellationToken);

            return result;
        }

        public async Task<HttpResponseMessage?> PostAsyncWithResponseMessage<T>(string url, T data, CancellationToken cancellationToken = default)
        {
            await SetTokens();

            var result = await httpClient.PostAsJsonAsync(url, data, cancellationToken);

            return result;
        }

        public async Task<string> GetUserRole()
        {
            var token = await GetAccessToken();
            var identity = GetIdentityFromToken($"{token?.Value}");
            var userId = identity?.GetUserId() ?? string.Empty;

            var idToken = await GetIdToken(userId);
            var idIdentity = GetIdentityFromToken(idToken ?? string.Empty);

            return idIdentity?.GetRoles() ?? string.Empty;
        }

        public async Task<(string, string)> GetTokens()
        {
            var accessToken = (await GetAccessToken())?.Value ?? string.Empty;
            var identity = GetIdentityFromToken($"{accessToken}");
            var userId = identity?.GetUserId() ?? string.Empty;

            var idToken = (await GetIdToken(userId)) ?? string.Empty;
            return (accessToken, idToken);
        }

        private async Task SetTokens()
        {
            var userId = await SetAccessToken();
            await SetIdToken(userId);
        }

        private async Task<string> SetAccessToken()
        {
            var token = await GetAccessToken();

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);
            }

            var identity = GetIdentityFromToken($"{token?.Value}");

            return identity?.GetUserId() ?? string.Empty;
        }

        private async Task SetIdToken(string userId)
        {
            var idToken = await GetIdToken(userId);

            if (!string.IsNullOrEmpty(idToken))
            {
                if (!httpClient.DefaultRequestHeaders.Contains(ClaimsPrincipalExtensions.IdTokenHeader))
                {
                    httpClient.DefaultRequestHeaders.Add(ClaimsPrincipalExtensions.IdTokenHeader, $"Bearer {idToken}");
                }
            }
        }

        private async Task<string> GetIdToken(string userId)
        {
            var userDataKey = $"{userId}.{configuration.GetValue<string>("IdToken:Identifier")}";
            var idTokenStr = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", [userDataKey]);

            IdTokenData idTokenData = default!;

            try
            {
                idTokenData = JsonSerializer.Deserialize<IdTokenData>(idTokenStr);
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return idTokenData!.secret;
        }

        private async Task<AccessToken?> GetAccessToken()
        {
            var accessTokenResult = await tokenProvider.RequestAccessToken();

            if (!accessTokenResult.TryGetToken(out var token))
            {
                return null;
            }

            return token;
        }

        private static ClaimsPrincipal? GetIdentityFromToken(string token)
        {
            var jst = JwtValidationHelper.ReadToken(token, null);

            if (jst is null)
            {
                return null;
            }

            var identity = new ClaimsPrincipal(new ClaimsIdentity(jst.Claims));

            return identity;
        }
    }

    public class IdTokenData
    {
        public string credentialType { get; set; } = default!;
        public string homeAccountId { get; set; } = default!;
        public string environment { get; set; } = default!;
        public string clientId { get; set; } = default!;
        public string secret { get; set; } = default!;
        public string realm { get; set; } = default!;
    }
}
