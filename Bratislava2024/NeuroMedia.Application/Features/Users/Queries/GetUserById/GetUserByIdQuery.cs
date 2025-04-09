using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ExternalConnectors;

using NeuroMedia.Application.Features.Patients.Commands.CreatePatient;
using NeuroMedia.Application.Features.Users.Helpers;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Interfaces.Services;
using NeuroMedia.Application.Logging;
using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(string Id, LogRow LogRow) : IRequest<GetUserByIdDto>;

    internal class GetUserByIdQueryHandler(IAzureUserService azureUserService, IConfiguration configuration, ILogger<GetUserByIdQueryHandler> logger) : IRequestHandler<GetUserByIdQuery, GetUserByIdDto>
    {
        private readonly IAzureUserService _azureUserService = azureUserService;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<GetUserByIdQueryHandler> _logger = logger;

        public async Task<GetUserByIdDto> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var institutionAttributeName = AzureCustomAttributeHelper.GetCompleteAttributeName("Institution", _configuration["AzureAd:B2cExtensionAppClientId"] ?? string.Empty);
            var rolesAttributeName = AzureCustomAttributeHelper.GetCompleteAttributeName("Roles", _configuration["AzureAd:B2cExtensionAppClientId"] ?? string.Empty);

            var result = await _azureUserService.GetAzureUser(query.Id,
                [institutionAttributeName, rolesAttributeName], cancellationToken);

            if (result == null)
            {
                _logger.LogWarningAndThrow404(query.LogRow, $"Azure user with ID {query.Id} was not found");
            }

            result!.AdditionalData.TryGetValue(institutionAttributeName, out var institution);
            result.AdditionalData.TryGetValue(rolesAttributeName, out var roles);

            return new GetUserByIdDto
            {

                Id = result.Id ?? string.Empty,
                GivenName = result.GivenName ?? string.Empty,
                Surname = result.Surname ?? string.Empty,
                DisplayName = result.DisplayName ?? string.Empty,
                Institution = institution?.ToString() ?? string.Empty,
                Roles = roles?.ToString() ?? string.Empty,
                Email = result.Mail ?? string.Empty
            };
        }
    }
}
