using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Interfaces.Repositories
{
    public interface IVideoRepository : IGenericRepository<Video>
    {
        Task<Video?> GetByVisitIdAndTypeAsync(int visitId, VideoType type);
        Task<Video?> GetByIdIncludeVisitAsync(int id);
    }
}
