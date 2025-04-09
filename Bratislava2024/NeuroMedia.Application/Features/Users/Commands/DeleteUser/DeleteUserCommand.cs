using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models.ODataErrors;
using Microsoft.Graph.Models;

using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;

using NeuroMedia.Domain.Entities;
using Microsoft.Extensions.Configuration;
using NeuroMedia.Application.Interfaces.Services;
using System.ComponentModel.Design;

namespace NeuroMedia.Application.Features.Users.Commands.DeleteUser
{
    public record DeleteUserCommand(string Id, LogRow LogRow) : IRequest;

    internal class DeleteUserCommandHandler(IAzureUserService azureUserService, ILogger<DeleteUserCommandHandler> logger) : IRequestHandler<DeleteUserCommand>
    {
        private readonly IAzureUserService _azureUserService = azureUserService;
        private readonly ILogger<DeleteUserCommandHandler> _logger = logger;

        public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                await _azureUserService.DeleteAzureUser(command.Id, cancellationToken);
            }
            catch (ODataError e) when (e.ResponseStatusCode.Equals(StatusCodes.Status403Forbidden))
            {
                _logger.LogWarningAndThrow403(e, command.LogRow, "Forbidden to delete Azure user");
            }
            catch (ODataError e) when (e.ResponseStatusCode.Equals(StatusCodes.Status404NotFound))
            {
                _logger.LogWarningAndThrow404(command.LogRow, $"Azure user with ID {command.Id} was not found", e);
            }
            catch (Exception e)
            {
                _logger.LogWarningAndThrow400(command.LogRow, $"Unable to delete Azure user", e);
            }
        }
    }
}
