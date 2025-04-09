using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Application.Features.Questionnaires.Dtos;

namespace NeuroMedia.Seed.Helpers.QuestionnaireGeneration
{
    public interface IQuestionnaireGeneration
    {
        QuestionnaireDto QuestionnaireDto { get; }
    }
}
