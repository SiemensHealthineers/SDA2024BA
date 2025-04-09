using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Graph.Models;

using NeuroMedia.Application.Features.Users.Helpers;
using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Application.Interfaces.Services;
using NeuroMedia.Application.Common.Helpers;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;
using System.Security.Claims;
using Microsoft.Graph.Models.ExternalConnectors;

namespace NeuroMedia.Application.Features.Users.Queries.GetAllUsers
{
    public record GetAllUsersQuery(ClaimsPrincipal Claims) : IRequest<List<GetAllUsersDto>>;

    internal class GetAllUsersQueryHandler(IAzureUserService azureUserService, IConfiguration configuration) : IRequestHandler<GetAllUsersQuery, List<GetAllUsersDto>>
    {
        private readonly IAzureUserService _azureUserService = azureUserService;
        private readonly IConfiguration _configuration = configuration;

        public async Task<List<GetAllUsersDto>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            var institutionAttributeName = AzureCustomAttributeHelper.GetCompleteAttributeName("Institution", _configuration["AzureAd:B2cExtensionAppClientId"] ?? string.Empty);
            var rolesAttributeName = AzureCustomAttributeHelper.GetCompleteAttributeName("Roles", _configuration["AzureAd:B2cExtensionAppClientId"] ?? string.Empty);

            var result = await _azureUserService.GetAllAzureUsers(
                        [institutionAttributeName, rolesAttributeName],
                        query.Claims.IsInRoleCustomImplementation($"{Roles.Admin}") ? query.Claims.GetTenantId() : string.Empty,
                        cancellationToken);

            var userDtos = result.Select(x =>
            {
                x.AdditionalData.TryGetValue(institutionAttributeName, out var institution);
                x.AdditionalData.TryGetValue(rolesAttributeName, out var roles);

                return new GetAllUsersDto
                {

                    Id = x.Id ?? string.Empty,
                    DisplayName = x.DisplayName ?? string.Empty,
                    Institution = institution?.ToString() ?? string.Empty,
                    Roles = roles?.ToString() ?? string.Empty
                };
            });

            return userDtos.ToList() ?? [];
        }
    }
}
