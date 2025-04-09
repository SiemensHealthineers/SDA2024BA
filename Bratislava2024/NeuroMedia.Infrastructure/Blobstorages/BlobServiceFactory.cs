using Azure.Storage.Blobs;

using NeuroMedia.Application.Interfaces.Blobstorages;

namespace NeuroMedia.Infrastructure.Blobstorages
{
    public class BlobServiceFactory : IBlobServiceFactory
    {
        public BlobServiceClient CreateClient(string connectionString)
        {
            return new BlobServiceClient(connectionString);
        }
    }
}
