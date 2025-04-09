using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Interfaces.Repositories
{
    public interface IQuestionnaireRepository : IGenericRepository<Questionnaire>
    {
        Task<Questionnaire?> GetByVisitIdAndTypeAsync(int visitId, QuestionnaireType type);
        Task<Questionnaire?> GetByIdIncludeVisitAsync(int id);
    }
}
