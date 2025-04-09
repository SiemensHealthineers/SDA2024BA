using System.Collections;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

using Microsoft.Maui.Storage;

using NeuroMediaMobileApp.Services.Interfaces;

namespace NeuroMediaMobileApp.Services
{
    public class VideoService : IVideoService
    {
        private readonly IHttpClientService _httpClient;

        public VideoService(IHttpClientService httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> UploadVideoFileAsync(string blobPath, Stream stream, string fileName)
        {
            var byteArray = GetBytesFromStream(stream);
            var base64Str = Convert.ToBase64String(byteArray);

            var videoDto = new VideoDto()
            {
                BlobPath = blobPath,
                ContentBase64 = base64Str
            };

            var postResponse = await _httpClient.PostAsJsonAsync($"Videos/upload", videoDto);

            return postResponse;
        }

        public class VideoDto
        {
            public string BlobPath { get; set; } = default!;
            public string ContentBase64 { get; set; } = default!;
        }

        private static byte[] GetBytesFromStream(Stream stream)
        {
            byte[] bytes;

            stream.Seek(0, SeekOrigin.Begin);

            using (var binaryReader = new BinaryReader(stream))
            {
                bytes = binaryReader.ReadBytes((int) stream.Length);
            }

            return bytes;
        }
    }
}
