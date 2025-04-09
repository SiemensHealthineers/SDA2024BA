using System.Net;
using System.Security.Claims;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using NeuroMedia.Application.Common.Helpers;
using NeuroMedia.Application.Exceptions;

namespace NeuroMedia.API.Controllers
{
    [ApiController]
    [Authorize]
    public class ApiController : ControllerBase
    {
        public async Task<IActionResult> MediatorTrySend<T>(IMediator mediator, IRequest<T> request)
        {
            try
            {
                var result = await mediator.Send(request);

                return (result != null)
                    ? Ok(result) : StatusCode((int) HttpStatusCode.NotFound, "Not found");
            }
            catch (Exception ex)
            {
                return StatusCode(GetStatusCode(ex), ex.Message);
            }
        }

        private static int GetStatusCode(Exception ex)
        {
            var exceptionType = ex.GetType();

            if (exceptionType == typeof(NotFoundClientException))
            {
                return (int) HttpStatusCode.NotFound;
            }
            else if (exceptionType == typeof(BadRequestClientException))
            {
                return (int) HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(ForbiddenClientException))
            {
                return (int) HttpStatusCode.Forbidden;
            }
            else if (exceptionType == typeof(UnauthorizedClientException))
            {
                return (int) HttpStatusCode.Unauthorized;
            }

            return (int) HttpStatusCode.InternalServerError;
        }

        protected ClaimsPrincipal GetUserWithCustomClaims<T>(HttpRequest request, IConfiguration configuration, ILogger<T> logger)
        {
            var useIdToken = bool.TrueString == configuration["Token:UseIdToken"];

            if (!useIdToken)
            {
                return User;
            }

            var requestHeaders = request.Headers;

            var token = useIdToken
                        ? (requestHeaders?.FirstOrDefault(x => x.Key == ClaimsPrincipalExtensions.IdTokenHeader))?.Value
                        : requestHeaders?.Authorization;

            if (token is null)
            {
                return User; // fallback
            }

            token = token.Value.ToString().Replace("Bearer", string.Empty).Trim();

            var jst = JwtValidationHelper.ReadToken($"{token}", logger); // reading should be enough, validation is done inside [Authorize]

            if (jst is null)
            {
                return User; // fallback
            }

            var identity = new ClaimsPrincipal(new ClaimsIdentity(jst.Claims));

            return identity;
        }
    }
}
