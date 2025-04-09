using System.Net;

namespace NeuroMedia.Application.Exceptions
{
    [Serializable]
    public class ClientException : Microsoft.Graph.ClientException
    {
        public ClientException(string message, int code, Exception exception = null) : base(message, exception)
        {
            ResponseStatusCode = code;
        }

        public ClientException(string message, Exception exception = null) : base(message, exception)
        {
            ResponseStatusCode = (int) HttpStatusCode.InternalServerError;
        }
    }
}
