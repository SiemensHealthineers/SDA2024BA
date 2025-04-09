using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using NeuroMediaMobileApp.Services.Interfaces;

namespace NeuroMediaMobileApp.Services;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;
    private readonly ISessionService _sessionService;

    public HttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _sessionService = ServiceHelper.Services.GetService<ISessionService>();
    }

    public async Task<T?> GetFromJsonAsync<T>(string url)
    {
        await SetTokens();
        return await _httpClient.GetFromJsonAsync<T>(url);
    }

    public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T data)
    {
        await SetTokens();
        return await _httpClient.PostAsJsonAsync(url, data);
    }

    private async Task SetTokens()
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _sessionService.AccessToken);

        if (!_httpClient.DefaultRequestHeaders.Contains("neuromediaidtoken"))
        {
            _httpClient.DefaultRequestHeaders.Add("neuromediaidtoken", $"Bearer {_sessionService.IdToken}");
        }
    }
}
