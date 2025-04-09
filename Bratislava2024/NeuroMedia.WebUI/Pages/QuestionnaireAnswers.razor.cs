using Microsoft.AspNetCore.Components;
using NeuroMedia.Application.Features.Questionnaires.Dtos;
using System.Net.Http.Json;

namespace NeuroMedia.WebUI.Pages
{
    public partial class QuestionnaireAnswers
    {
        [Inject]
        public HttpClient HttpClient { get; set; } = default!;

        [Parameter]
        public QuestionnaireDto? Questionnaire { get; set; }

        [Parameter]
        public AnswersDto? Answers { get; set; }
    }
}
