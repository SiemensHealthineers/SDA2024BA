using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroMedia.Application.Features.Users.Helpers
{
    public static class AzureCustomAttributeHelper
    {
        public static string GetCompleteAttributeName(string attributeName, string azureExtensionAppClientId)
        {
            azureExtensionAppClientId = azureExtensionAppClientId.Replace("-", "");

            if (string.IsNullOrWhiteSpace(attributeName))
            {
                throw new ArgumentException("Parameter cannot be null", nameof(attributeName));
            }

            return $"extension_{azureExtensionAppClientId}_{attributeName}";
        }
    }
}
