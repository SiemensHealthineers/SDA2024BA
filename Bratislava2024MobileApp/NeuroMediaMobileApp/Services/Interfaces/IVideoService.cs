using System.Threading.Tasks;

namespace NeuroMediaMobileApp.Services.Interfaces
{
    public interface IVideoService
    {
        public Task<HttpResponseMessage> UploadVideoFileAsync(string blobPath, Stream stream, string fileName);
    }
}
