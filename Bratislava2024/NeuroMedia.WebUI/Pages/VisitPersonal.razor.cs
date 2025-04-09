using System.Net.Http.Json;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using NeuroMedia.Application.Features.Patients.Queries.GetPatientById;
using NeuroMedia.Application.Features.Questionnaires.Dtos;
using NeuroMedia.Application.Features.VisitDetails.Queries.GetVisitDetailsById;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Domain.Enums;
using NeuroMedia.WebUI.Services.Interfaces;

namespace NeuroMedia.WebUI.Pages
{
    public partial class VisitPersonal
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        [Inject]
        public IHttpClientService HttpClientService { get; set; } = default!;
        [Parameter]
        public int PatientId { get; set; }
        [Parameter]
        public int VisitId { get; set; }
        public List<QuestionnaireRecordDto> QuestionnairesList { get; set; } = [];

        private AnswersDto? CurrentAnswers { get; set; }
        private QuestionnaireDto? SelectedQuestionnaire { get; set; }
        public string AccessToken { get; set; } = default!;
        public string IdToken { get; set; } = default!;
        public GetVisitDetailsByIdDto? Visit { get; set; }

        private GetPatientByIdDto? patient;
        private string? errorMessage;

        private async Task ShowQuestionnaireModal(QuestionnaireRecordDto questionnaire)
        {
            CurrentAnswers = await GetAnswersByBlobPathAsync(questionnaire.BlobPath);
            SelectedQuestionnaire = await GetQuestionnaireByTypeAsync((int) questionnaire.QuestionnaireType);

            if (JSRuntime == null)
            {
                throw new InvalidOperationException("JSRuntime is not initialized.");
            }
            await JSRuntime.InvokeVoidAsync("ShowModal", "questionnaireAnswersModal");
        }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                QuestionnairesList = await HttpClientService.GetAsync<List<QuestionnaireRecordDto>>($"api/Questionnaires/Results/{VisitId}");

                Visit = await GetVisitDetails(PatientId, VisitId);
                patient = await GetPatientById(PatientId);
                (AccessToken, IdToken) = await HttpClientService.GetTokens();

                if (Visit == null || patient == null)
                {
                    errorMessage = "Sorry, there's nothing at this address.";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"An error occurred while loading the data. Please try again later. {ex.Message}";
            }
        }

        public async Task<GetVisitDetailsByIdDto?> GetVisitDetails(int patientId, int visitId)
        {
            try
            {
                return await HttpClientService.GetAsync<GetVisitDetailsByIdDto>($"api/VisitDetails/{patientId}/{visitId}");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<GetPatientByIdDto?> GetPatientById(int patientId)
        {
            try
            {
                return await HttpClientService.GetAsync<GetPatientByIdDto>($"api/Patients/{patientId}");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<AnswersDto?> GetAnswersByBlobPathAsync(string blobPath)
        {
            return await HttpClientService.GetAsync<AnswersDto>($"api/Questionnaires/Results?blobPath={blobPath}");
        }

        public async Task<QuestionnaireDto?> GetQuestionnaireByTypeAsync(int type)
        {
            return await HttpClientService.GetAsync<QuestionnaireDto>($"api/Questionnaires/{type}");
        }

        private async Task ShowQuestionnaireAnswers()
        {
            if (JSRuntime == null)
            {
                throw new InvalidOperationException("JSRuntime is not initialized.");
            }
            await JSRuntime.InvokeVoidAsync("ShowModal", "questionnaireAnswersModal");
        }
    }
}
