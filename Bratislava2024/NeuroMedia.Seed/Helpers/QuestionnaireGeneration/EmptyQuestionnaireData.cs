using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Application.Features.Questionnaires.Helpers;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Seed.Helpers.QuestionnaireGeneration
{
    public class EmptyQuestionnaireData(QuestionnaireType questionnaireType) : IQuestionnaireGeneration
    {
        public QuestionnaireDto QuestionnaireDto { get; } = new()
        {
            BlobPath = $"{QuestionnaireHelper.QuestionnaireFolder}/{questionnaireType}{QuestionnaireHelper.QuestionnaireFileExt}",
            Questions = [
                new QuestionDto
                {
                    Id = 1,
                    Type = "Radio",
                    Text = "Empty question 1:",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "Empty answer 1",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "Empty answer 2",
                            Value = "2"
                        }
                    ]
                }
            ]
        };
    }
}
