using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NeuroMedia.Application.Interfaces.Blobstorages;
using NeuroMedia.Application.Logging;
using NeuroMedia.Application.Exceptions;

namespace NeuroMedia.Infrastructure.Blobstorages
{
    public class BlobStorage : IBlobStorage
    {
        private readonly ILogger<BlobStorage> _logger;

        public BlobStorage(IBlobServiceFactory blobServiceFactory, IConfiguration configuration, ILogger<BlobStorage> logger)
        {
            _logger = logger;

            var containerName = configuration["BlobStorageSettings:ContainerName"];
            var containerClient = blobServiceFactory.CreateClient(configuration["BlobStorageSettings:ConnectionString"]!)
                .GetBlobContainerClient(containerName);
            containerClient.CreateIfNotExists();

            ContainerClient = containerClient;
        }

        public BlobContainerClient ContainerClient { get; }

        public Task<Response<bool>> DropContainer(BlobContainerClient blobContainer)
        {
            var newLogRow = new LogRow { Message = $"{nameof(DropContainer)} execution." }.UpdateCallerProperties();
            _logger.LogInformation(newLogRow.UpdateCallerProperties());

            try
            {
                return blobContainer.DeleteIfExistsAsync();
            }
            catch (RequestFailedException e)
            {
                newLogRow.Message = $"{nameof(DropContainer)} exception.";
                _logger.LogErrorAndThrow500(newLogRow.UpdateCallerProperties(), "Failed to delete blob container.", e);
            }

            return default!;
        }

        public IAsyncEnumerable<Page<BlobItem>> GetAllByPrefixAsync(string prefix, LogRow? logRow = null)
        {
            var newLogRow = new LogRow(logRow)
            {
                Message = $"{nameof(GetAllByPrefixAsync)} execution.",
                LogData = $"Prefix: {prefix}"
            };
            _logger.LogInformation(newLogRow.UpdateCallerProperties());

            return ContainerClient.GetBlobsAsync(BlobTraits.Metadata, prefix: prefix)
                .AsPages();
        }

        public BlobItem GetSingleByPrefix(string prefix, LogRow? logRow = null)
        {
            var newLogRow = new LogRow(logRow)
            {
                Message = $"{nameof(GetSingleByPrefix)} execution.",
                LogData = $"Prefix: {prefix}"
            };
            _logger.LogInformation(newLogRow.UpdateCallerProperties());

            return ContainerClient.GetBlobs(prefix: prefix)
                .Single();
        }

        public async Task<bool> UploadAsync(string path, Stream content, IDictionary<string, string>? metadata = null,
            bool silentContinue = true, bool overWrite = false, LogRow? logRow = null)
        {
            var newLogRow = new LogRow(logRow)
            {
                Message = $"{nameof(UploadAsync)} execution.",
                LogData = $"Path: {path}"
            };
            _logger.LogInformation(newLogRow.UpdateCallerProperties());

            try
            {
                var blobClient = ContainerClient.GetBlobClient(path);
                await blobClient.UploadAsync(content, overWrite);

                if (metadata != null)
                {
                    await blobClient.SetMetadataAsync(metadata);
                }
            }
            catch (RequestFailedException e) when ("BlobAlreadyExists".Equals(e.ErrorCode, StringComparison.Ordinal))
            {
                if (silentContinue)
                {
                    return false;
                }

                throw;
            }
            catch (Exception e) when (e is not RequestFailedException { ErrorCode: "BlobAlreadyExists" })
            {
                return false;
            }

            return true;
        }

        public async Task<Stream> DownloadAsync(string path, LogRow? logRow = null)
        {
            var newLogRow = new LogRow(logRow)
            {
                Message = $"{nameof(DownloadAsync)} execution.",
                LogData = $"Path: {path}"
            };
            _logger.LogInformation(newLogRow.UpdateCallerProperties());

            try
            {
                var blobClient = ContainerClient.GetBlobClient(path);

                if (!(await blobClient.ExistsAsync()).Value)
                {
                    _logger.LogWarningAndThrow400(newLogRow.UpdateCallerProperties(), "Blob does not exist");
                }

                var stream = new MemoryStream();
                await blobClient.DownloadToAsync(stream);
                stream.Seek(0, SeekOrigin.Begin);

                return stream;
            }
            catch (Exception e) when (e is not ClientException)
            {
                _logger.LogErrorAndThrow500(newLogRow.UpdateCallerProperties(), "Failed to download blob due to an exception.", e);
            }

            return Stream.Null;
        }

        public async Task<bool> DeleteAsync(string path, LogRow? logRow = null)
        {
            var newLogRow = new LogRow(logRow)
            {
                Message = $"{nameof(DeleteAsync)} execution.",
                LogData = $"Path: {path}"
            };
            _logger.LogInformation(newLogRow.UpdateCallerProperties());

            return await ContainerClient.DeleteBlobIfExistsAsync(path);
        }
    }
}
