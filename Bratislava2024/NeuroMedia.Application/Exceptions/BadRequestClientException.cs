using System.Net;

namespace NeuroMedia.Application.Exceptions
{
    public class BadRequestClientException(string message, Exception exception = null) : ClientException(message, (int) HttpStatusCode.BadRequest, exception)
    {
    }
}
