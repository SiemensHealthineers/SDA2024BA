using System.ComponentModel.Design;
using System.Security.Claims;

using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models.ODataErrors;
using Microsoft.Graph.Models;

using NeuroMedia.Application.Common.Helpers;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;

using NeuroMedia.Domain.Entities;
using Microsoft.Extensions.Configuration;
using NeuroMedia.Application.Interfaces.Services;
using NeuroMedia.Application.Features.Users.Helpers;

namespace NeuroMedia.Application.Features.Users.Commands.UpdateUser
{
    public record UpdateUserCommand(int Id, UpdateUserDto Dto, ClaimsPrincipal Claims, LogRow LogRow) : IRequest<UpdateUserDto>;

    internal class UpdateUserCommandHandler(IAzureUserService azureUserService, IConfiguration configuration, ILogger<UpdateUserCommandHandler> logger) : IRequestHandler<UpdateUserCommand, UpdateUserDto>
    {
        private readonly IAzureUserService _azureUserService = azureUserService;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<UpdateUserCommandHandler> _logger = logger;

        public async Task<UpdateUserDto> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var azureDomain = _configuration["AzureAd:Domain"] ?? string.Empty;
            var institutionAttributeName = AzureCustomAttributeHelper.GetCompleteAttributeName("Institution", _configuration["AzureAd:B2cExtensionAppClientId"] ?? string.Empty);
            var rolesAttributeName = AzureCustomAttributeHelper.GetCompleteAttributeName("Roles", _configuration["AzureAd:B2cExtensionAppClientId"] ?? string.Empty);

            var mappedUserToUpdate = new User
            {
                Id = command.Dto.Id,
                GivenName = command.Dto.GivenName,
                Surname = command.Dto.Surname,
                DisplayName = command.Dto.DisplayName,
                Mail = command.Dto.Email,
                Identities =
                [
                    new ()
                    {
                        SignInType = "emailAddress",
                        Issuer = azureDomain,
                        IssuerAssignedId = command.Dto.Email
                    }
                ],
            };

            mappedUserToUpdate.AdditionalData.Add(institutionAttributeName, command.Dto.Institution);
            mappedUserToUpdate.AdditionalData.Add(rolesAttributeName, command.Dto.Roles);

            try
            {
                await _azureUserService.UpdateAzureUser(mappedUserToUpdate, cancellationToken);
            }
            catch (ODataError e) when (e.ResponseStatusCode.Equals(StatusCodes.Status403Forbidden))
            {
                _logger.LogWarningAndThrow403(e, command.LogRow, "Forbidden to update Azure user");
            }
            catch (ODataError e) when (e.ResponseStatusCode.Equals(StatusCodes.Status404NotFound))
            {
                _logger.LogWarningAndThrow404(command.LogRow, $"Azure user with email {command.Dto.Email} was not found", e);
            }
            catch (Exception e)
            {
                _logger.LogWarningAndThrow400(command.LogRow, $"Unable to update Azure user", e);
            }

            return default!;

        }
    }
}
