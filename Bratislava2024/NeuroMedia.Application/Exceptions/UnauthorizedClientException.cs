using System.Net;

namespace NeuroMedia.Application.Exceptions
{
    public class UnauthorizedClientException(string message, Exception exception = null) : ClientException(message, (int) HttpStatusCode.Unauthorized, exception)
    {
    }
}
