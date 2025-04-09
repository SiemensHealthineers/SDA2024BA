using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using NeuroMedia.Application.Logging;

using Newtonsoft.Json;

namespace NeuroMedia.Application.Common.Helpers
{
    public static class JwksHelper
    {
        private const int ValidDurationInHours = 24;

        private static readonly DateTime s_lastUpdated = DateTime.MinValue;

        private static readonly Dictionary<string, JwksData> s_kidDataPairs = [];

        public static async Task<JwksData> GetKidDataPair(string kid, IConfiguration configuration, ILogger logger,
            LogRow? logRow = null)
        {
            var jwksUri = configuration["Token:JwksUri"];

            await UpdateKidDataPairsAsync(jwksUri!, logger, logRow);

            return s_kidDataPairs.TryGetValue(kid, out var data) ? data : new JwksData();
        }

        private static async Task UpdateKidDataPairsAsync(string jwksUri, ILogger logger, LogRow? logRow = null)
        {
            if (DateTime.Now >= s_lastUpdated.AddHours(ValidDurationInHours) || s_kidDataPairs.Count == 0)
            {
                var jwksData = await DownloadJwksData(jwksUri, logger, logRow);

                dynamic? data = JsonConvert.DeserializeObject(jwksData);

                try
                {
                    var firstTry = true;

                    foreach (var key in data!.keys)
                    {
                        string kid = key.kid;
                        string modulus = key.n;
                        string exponent = key.e;

                        if (!string.IsNullOrEmpty(kid))
                        {
                            if (firstTry)
                            {
                                s_kidDataPairs.Clear();
                                firstTry = false;
                            }

                            s_kidDataPairs.Add(kid, new JwksData
                            {
                                Modulus = modulus,
                                Exponent = exponent
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    var newLogRow = new LogRow(logRow).UpdateCallerProperties();
                    newLogRow.Message = $"{nameof(UpdateKidDataPairsAsync)} exception";
                    logger.LogError(new EventId(), ex, newLogRow.Message);
                }
            }
        }

        private static async Task<string> DownloadJwksData(string url, ILogger logger, LogRow? logRow = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            using var client = new HttpClient();

            try
            {
                return await client.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                var newLogRow = new LogRow(logRow).UpdateCallerProperties();
                newLogRow.Message = $"{nameof(DownloadJwksData)} exception";
                logger.LogError(new EventId(), ex, newLogRow.Message);
            }

            return string.Empty;
        }
    }

    public class JwksData
    {
        public string Modulus { get; init; } = string.Empty;
        public string Exponent { get; init; } = string.Empty;
    }

    public class KidData
    {
        public List<KidKeys> Keys { get; init; } = default!;
    }

    public class KidKeys
    {
        public string Kty { get; init; } = default!;
        public string Use { get; init; } = default!;
        public string Kid { get; init; } = default!;
        public string X5t { get; init; } = default!;
        public string N { get; init; } = default!;
        public string E { get; init; } = default!;
        public string X5c { get; init; } = default!;
        public string Issuer { get; init; } = default!;
    }
}
