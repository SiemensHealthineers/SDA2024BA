using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroMedia.Application.Common.Helpers
{
    public static class Base64Helper
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static byte[] FromBase64Url(string base64Url)
        {
            var padded = base64Url.Length % 4 == 0 ?
                            base64Url :
                            string.Concat(base64Url, "====".AsSpan(base64Url.Length % 4));

            var base64 = padded.Replace("_", "/")
                               .Replace("-", "+");

            return Convert.FromBase64String(base64);
        }
    }
}
