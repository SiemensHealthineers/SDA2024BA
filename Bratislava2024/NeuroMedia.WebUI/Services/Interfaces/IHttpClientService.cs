using Microsoft.Graph.Drives.Item.Items.Item.GetActivitiesByInterval;

namespace NeuroMedia.WebUI.Services.Interfaces
{
    public interface IHttpClientService
    {
        public Task<List<T>> GetListAsync<T>(string url, CancellationToken cancellationToken = default);
        public Task<T?> GetAsync<T>(string url, CancellationToken cancellationToken = default);
        public Task<bool> PostAsync<T>(string url, T data, CancellationToken cancellationToken = default);
        public Task<bool> PutAsync<T>(string url, T data, CancellationToken cancellationToken = default);
        public Task<bool> DeleteAsync(string url, CancellationToken cancellationToken = default);
        public Task<string> GetUserRole();
        public Task<(string, string)> GetTokens();
        public Task<bool> PutEmptyAsync(string url, CancellationToken cancellationToken = default);
        public Task<HttpResponseMessage?> PutAsyncWithResponseMessage<T>(string url, T data, CancellationToken cancellationToken = default);
        public Task<HttpResponseMessage?> PostAsyncWithResponseMessage<T>(string url, T data, CancellationToken cancellationToken = default);
    }
}
