using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Graph.Models;
using Microsoft.Graph;

using NeuroMedia.Application.Interfaces.Services;

namespace NeuroMedia.Application.Features.Users.Services
{
    public class AzureUserService(GraphServiceClient? graphClient) : IAzureUserService
    {
        private readonly GraphServiceClient? _graphClient = graphClient;

        //public AzureUserService() : this(null)
        //{
        //}

        public async Task<IList<User>> GetAllAzureUsers(string[] customAttributes, string? tenantId,
            CancellationToken cancellationToken)
        {
            var userAttributes = new List<string>
            {
                "id", "displayName", "identities",
            };

            if (_graphClient == null)
            {
                return [];
            }

            try
            {
                var response = await _graphClient.Users.GetAsync(
                    request =>
                    {
                        request.QueryParameters.Select = [.. userAttributes, .. customAttributes];

                        // TODO: uncomment when tenant is available

                        if (string.IsNullOrEmpty(tenantId))
                        {
                            return;
                        }

                        var institutionAttribute = customAttributes.FirstOrDefault(x => x.Contains("Institution"));

                        var filterQuery = string.Empty;
                        if (!string.IsNullOrEmpty(institutionAttribute))
                        {
                            filterQuery += $"{institutionAttribute} eq '{tenantId}'";
                        }
                        else
                        {
                            throw new InvalidOperationException("CustomInstitutionAttribute cannot be empty");
                        }

                        request.QueryParameters.Filter = filterQuery;
                    },
                    cancellationToken);

                return response?.Value?.ToList() ?? new List<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return [];
        }

        public async Task<User?> GetAzureUser(string userId, string[] customAttributes, CancellationToken cancellationToken)
        {
            var userAttributes = new List<string>
        {
            "id",
            "displayName",
            "identities",
            "givenName",
            "surname",
            "mail"
        };

            if (_graphClient == null)
            {
                return null;
            }

            return await _graphClient.Users[userId].GetAsync(
                request => request.QueryParameters.Select = userAttributes.Concat(customAttributes).ToArray(),
                cancellationToken);
        }

        public async Task UpdateAzureUser(User userToUpdate, CancellationToken cancellationToken)
        {
            if (_graphClient == null)
            {
                return;
            }

            await _graphClient.Users[userToUpdate.Id].PatchAsync(userToUpdate, null, cancellationToken);
        }

        public async Task<User?> CreateAzureUser(User userToCreate, CancellationToken cancellationToken)
        {
            if (_graphClient == null)
            {
                return null;
            }

            return await _graphClient.Users.PostAsync(userToCreate, null, cancellationToken);
        }

        public async Task DeleteAzureUser(string userId, CancellationToken cancellationToken)
        {
            if (_graphClient == null)
            {
                return;
            }

            await _graphClient.Users[userId].DeleteAsync(null, cancellationToken);
        }
    }
}
