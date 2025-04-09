using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using NeuroMedia.Application.Logging;

namespace NeuroMedia.Application.Interfaces.Blobstorages
{
    public interface IBlobStorage
    {
        BlobContainerClient ContainerClient { get; }
        Task<Response<bool>> DropContainer(BlobContainerClient blobContainer);
        IAsyncEnumerable<Page<BlobItem>> GetAllByPrefixAsync(string prefix, LogRow? logRow = null);
        BlobItem GetSingleByPrefix(string prefix, LogRow? logRow = null);
        Task<bool> UploadAsync(string path, Stream content, IDictionary<string, string>? metadata = null,
            bool silentContinue = true, bool overWrite = false, LogRow? logRow = null);
        Task<Stream> DownloadAsync(string path, LogRow? logRow = null);
        Task<bool> DeleteAsync(string path, LogRow? logRow = null);
    }
}
