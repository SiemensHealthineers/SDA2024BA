using System.Net;

namespace NeuroMedia.Application.Exceptions
{
    public class ForbiddenClientException(string message, Exception exception = null) : ClientException(message, (int) HttpStatusCode.Forbidden, exception)
    {
    }
}
