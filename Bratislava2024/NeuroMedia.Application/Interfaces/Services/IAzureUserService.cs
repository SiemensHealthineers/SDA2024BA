using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace NeuroMedia.Application.Interfaces.Services
{
    public interface IAzureUserService
    {
        Task<IList<User>> GetAllAzureUsers(string[] customAttributes, string? tenantId, CancellationToken cancellationToken);
        Task<User?> GetAzureUser(string userId, string[] customAttributes, CancellationToken cancellationToken);
        Task UpdateAzureUser(User userToUpdate, CancellationToken cancellationToken);
        Task<User?> CreateAzureUser(User userToCreate, CancellationToken cancellationToken);
        Task DeleteAzureUser(string userId, CancellationToken cancellationToken);
    }
}
