using System.Net;

namespace NeuroMedia.Application.Exceptions
{
    public class NotFoundClientException(string message, Exception exception = null) : ClientException(message, (int) HttpStatusCode.NotFound, exception)
    {
    }
}
