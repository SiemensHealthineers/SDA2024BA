using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Domain.Enums;
using NeuroMedia.Seed.Helpers.QuestionnaireGeneration;

namespace NeuroMedia.Seed.Helpers
{
    public static class QuestionnaireGenerationHelper
    {
        public static QuestionnaireDto Generate(QuestionnaireType questionnaireType)
        {
            return questionnaireType switch
            {
                QuestionnaireType.Botulotoxin => new EmptyQuestionnaireData(questionnaireType).QuestionnaireDto,
                QuestionnaireType.CGIDoctor => new CgiDoctorQuestionnaireData().QuestionnaireDto,
                QuestionnaireType.CGIPatient => new CgiPatientQuestionnaireData().QuestionnaireDto,
                QuestionnaireType.TWSTRSDoctor => new TWSTRSDoctorData().QuestionnaireDto,
                QuestionnaireType.TWSTRSPatient => new TWSTRSPatientQuestionnaireData().QuestionnaireDto,
                _ => new EmptyQuestionnaireData(questionnaireType).QuestionnaireDto
            };
        }
    }
}
