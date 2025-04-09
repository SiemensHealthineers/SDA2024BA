using System.Security.Claims;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models.ODataErrors;
using Microsoft.Graph.Models;

using NeuroMedia.Application.Common.Helpers;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NeuroMedia.Application.Features.Users.Queries.GetUserById;
using NeuroMedia.Application.Interfaces.Services;
using NeuroMedia.Application.Features.Users.Helpers;

namespace NeuroMedia.Application.Features.Users.Commands.CreateUser
{
    public record CreateUserCommand(CreateUserDto Dto, ClaimsPrincipal Claims, LogRow LogRow) : IRequest<CreateUserDto>;

    internal class CreateUserCommandHandler(IAzureUserService azureUserService, IConfiguration configuration, ILogger<CreateUserCommandHandler> logger) : IRequestHandler<CreateUserCommand, CreateUserDto>
    {
        private readonly IAzureUserService _azureUserService = azureUserService;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<CreateUserCommandHandler> _logger = logger;

        public async Task<CreateUserDto> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var mailNickName = command.Dto.Email.Split("@")[0];

            var azureDomain = _configuration["AzureAd:Domain"] ?? string.Empty;
            var institutionAttributeName = AzureCustomAttributeHelper.GetCompleteAttributeName("Institution", _configuration["AzureAd:B2cExtensionAppClientId"] ?? string.Empty);
            var rolesAttributeName = AzureCustomAttributeHelper.GetCompleteAttributeName("Roles", _configuration["AzureAd:B2cExtensionAppClientId"] ?? string.Empty);

            var mappedUserToCreate = new User
            {
                AccountEnabled = command.Dto.AccountEnabled,
                GivenName = command.Dto.GivenName,
                Surname = command.Dto.Surname,
                DisplayName = command.Dto.DisplayName,
                UserPrincipalName = $"{mailNickName}@{azureDomain}",
                MailNickname = mailNickName,
                PasswordProfile = new PasswordProfile
                {
                    Password = command.Dto.Password,
                    ForceChangePasswordNextSignIn = true
                },
                Identities =
                [
                    new ()
                    {
                        SignInType = "emailAddress",
                        Issuer = azureDomain,
                        IssuerAssignedId = command.Dto.Email
                    }
                ],
                Mail = command.Dto.Email
            };

            mappedUserToCreate.AdditionalData.Add(institutionAttributeName, command.Dto.Institution);
            mappedUserToCreate.AdditionalData.Add(rolesAttributeName, command.Dto.Roles);

            try
            {
                var result = await _azureUserService.CreateAzureUser(mappedUserToCreate, cancellationToken);

                return command.Dto; //TODO: map from result
            }
            catch (ODataError e) when (e.ResponseStatusCode.Equals(StatusCodes.Status403Forbidden))
            {
                _logger.LogWarningAndThrow403(e, command.LogRow, "Forbidden to create Azure user");
            }
            catch (ODataError e) when (e.ResponseStatusCode.Equals(StatusCodes.Status404NotFound))
            {
                _logger.LogWarningAndThrow404(command.LogRow, $"Azure user with email {command.Dto.Email} was not found", e);
            }
            catch (Exception e)
            {
                _logger.LogWarningAndThrow400(command.LogRow, $"Unable to create Azure user", e);
            }

            return default!;
        }
    }
}
