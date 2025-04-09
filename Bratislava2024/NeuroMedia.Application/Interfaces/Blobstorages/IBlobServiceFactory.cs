using Azure.Storage.Blobs;

namespace NeuroMedia.Application.Interfaces.Blobstorages
{
    public interface IBlobServiceFactory
    {
        BlobServiceClient CreateClient(string connectionString);
    }
}
