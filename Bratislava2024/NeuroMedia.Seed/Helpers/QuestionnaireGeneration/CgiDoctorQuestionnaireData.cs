using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Graph.Education.Classes.Item.Assignments.Item.Submissions.Item.Return;

using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Application.Features.Questionnaires.Helpers;
using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Seed.Helpers.QuestionnaireGeneration
{
    public class CgiDoctorQuestionnaireData : IQuestionnaireGeneration
    {
        public QuestionnaireDto QuestionnaireDto => s_questionnaire;

        private static readonly QuestionnaireDto s_questionnaire = new()
        {
            BlobPath = $"{QuestionnaireHelper.QuestionnaireFolder}/{QuestionnaireType.CGIDoctor}{QuestionnaireHelper.QuestionnaireFileExt}",
            Questions = [
                new QuestionDto
                {
                    Id = 1,
                    Type = "Radio",
                    Text = "Stupeň závažnosti cervikálnej dystónie je podľa vás:",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "extrémne chorý",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "závažne chorý",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "zjavne chorý",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "stredne chorý",
                            Value = "4"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "mierne chorý",
                            Value = "5"
                        },
                        new OptionDto
                        {
                            Id = 6,
                            Text = "hranične chorý",
                            Value = "6"
                        },
                        new OptionDto
                        {
                            Id = 7,
                            Text = "zdravý",
                            Value = "7"
                        }
                    ]
                },

                new QuestionDto
                {
                    Id = 2,
                    Type = "Radio",
                    Text = "Stav pacienta súvisiaci s cervikálnou dystóniou je po podaní botulotoxínu:",
                    Options = [
                        new OptionDto
                        {
                            Id = 1,
                            Text = "veľmi zlepšený",
                            Value = "1"
                        },
                        new OptionDto
                        {
                            Id = 2,
                            Text = "zlepšený",
                            Value = "2"
                        },
                        new OptionDto
                        {
                            Id = 3,
                            Text = "minimálne zlepšený",
                            Value = "3"
                        },
                        new OptionDto
                        {
                            Id = 4,
                            Text = "bez zmeny",
                            Value = "4"
                        },
                        new OptionDto
                        {
                            Id = 5,
                            Text = "minimálne zhoršený",
                            Value = "5"
                        },
                        new OptionDto
                        {
                            Id = 6,
                            Text = "zhoršený",
                            Value = "6"
                        },
                        new OptionDto
                        {
                            Id = 7,
                            Text = "výrazne zhoršený",
                            Value = "7"
                        }
                    ]
                }
            ]
        };
    }
}
