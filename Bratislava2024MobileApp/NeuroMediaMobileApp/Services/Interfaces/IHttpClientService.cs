using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroMediaMobileApp.Services.Interfaces
{
    public interface IHttpClientService
    {
        Task<T?> GetFromJsonAsync<T>(string url);
        Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T data);
    }
}
