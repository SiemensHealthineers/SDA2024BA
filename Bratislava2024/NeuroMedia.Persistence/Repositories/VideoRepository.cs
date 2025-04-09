using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using NeuroMedia.Application.Interfaces.Repositories;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;
using NeuroMedia.Persistence.Contexts;

namespace NeuroMedia.Persistence.Repositories
{
    public class VideoRepository(ApplicationDbContext dbContext) : GenericRepository<Video>(dbContext), IVideoRepository
    {
        public async Task<Video?> GetByIdIncludeVisitAsync(int id)
        {
            return await _dbContext.Videos
                .Include(x => x.Visit)
                .SingleOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Video?> GetByVisitIdAndTypeAsync(int visitId, VideoType type)
        {
            return await _dbContext.Videos
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(q => q.VisitId == visitId && q.VideoType == type);
        }
    }
}
