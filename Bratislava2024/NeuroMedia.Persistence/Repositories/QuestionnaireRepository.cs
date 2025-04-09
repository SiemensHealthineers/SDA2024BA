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
    public class QuestionnaireRepository(ApplicationDbContext dbContext) : GenericRepository<Questionnaire>(dbContext), IQuestionnaireRepository
    {
        public async Task<Questionnaire?> GetByIdIncludeVisitAsync(int id)
        {
            return await _dbContext.Questionnaires
                    .Include(x => x.Visit)
                    .SingleOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Questionnaire?> GetByVisitIdAndTypeAsync(int visitId, QuestionnaireType type)
        {
            return await _dbContext.Questionnaires
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(q => q.VisitId == visitId && q.QuestionnaireType == type);
        }
    }
}
