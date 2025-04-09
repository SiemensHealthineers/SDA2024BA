using NeuroMedia.Application.Features.Patients.Queries.GetPatientById;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NeuroMedia.WebUI.Models;
using NeuroMedia.Domain.Entities;
using NeuroMedia.Application.Features.Visits.Commands.CreateVisit;
using System.Net.Http.Json;
using NeuroMedia.Domain.Enums;
using NeuroMedia.WebUI.Services.Interfaces;
using NeuroMedia.Application.Features.Questionnaires.Commands.CreateQuestionnaireRecordsCommand;

namespace NeuroMedia.WebUI.Pages
{
    public partial class AddVisitModal
    {
        [Inject]
        private IHttpClientService HttpClientService { get; set; } = default!;
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Parameter]
        public EventCallback<VisitDataTransfer> OnVisitAdded { get; set; }
        [Parameter]
        public GetPatientByIdDto? InputPatient { get; set; }
        private Visit Visit { get; set; } = new Visit()
        {
            DateOfVisit = DateTime.Today
        };
        private bool AddOnlineVisit { get; set; }
        private bool IsBotulotoxin { get; set; }
        private bool IsCGIDoctor { get; set; }
        private bool IsCGIPatient { get; set; }
        public bool IsTWSTRSDoctor { get; set; }
        public bool IsTWSTRSPatient { get; set; }
        public bool IsTWSTRSPatientOnline { get; set; }
        public CreateVisitDto Result { get; set; }
        private async Task OnClose()
        {
            await JSRuntime.InvokeVoidAsync("closeModal", "addVisitModal");
        }



        private async Task HandleValidSubmit()
        {
            IsTWSTRSDoctor = true;

            if (Visit.VisitType == VisitType.Personal)
            {
                Result = await CreateVisit(Visit.DateOfVisit, Visit.VisitType);
                await AddQuestionnairesForPersonal();
                if (AddOnlineVisit)
                {
                    IsCGIPatient = true;
                    IsCGIDoctor = true;
                    var onlineVisitDate = Visit.DateOfVisit.AddDays(30);
                    Result = await CreateVisit(onlineVisitDate, VisitType.Online);
                    await AddQuestionnairesForOnline();
                }
            }
            else if (Visit.VisitType == VisitType.Online)
            {
                IsCGIPatient = true;
                IsCGIDoctor = true;
                Result = await CreateVisit(Visit.DateOfVisit, Visit.VisitType);
                await AddQuestionnairesForOnline();
            }
            await JSRuntime.InvokeVoidAsync("closeModal", "addVisitModal");
        }

        private async Task<CreateVisitDto> CreateVisit(DateTime dateOfVisit, VisitType visitType)
        {
            var dto = new CreateVisitDto
            {
                PatientId = InputPatient.Id,
                DateOfVisit = DateTime.SpecifyKind(dateOfVisit, DateTimeKind.Utc),
                VisitType = visitType,
                VisitId = Visit.Id,
            };

            var response = await HttpClientService.PostAsyncWithResponseMessage("api/PatientsVisits", dto);
            var responseData = await response.Content.ReadFromJsonAsync<CreateVisitDto>();
            if (response.IsSuccessStatusCode)
            {
                var newVisit = new VisitDataTransfer { DateOfVisit = dateOfVisit, VisitType = visitType };
                await OnVisitAdded.InvokeAsync(newVisit);
            }
            return responseData;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Visit.DateOfVisit = DateTime.Today;
            AddOnlineVisit = true;
        }
        public async Task AddQuestionnairesForPersonal()
        {
            var questionnairesToCreate = new Dictionary<QuestionnaireType, bool>
            {
            { QuestionnaireType.Botulotoxin, IsBotulotoxin },
            { QuestionnaireType.TWSTRSDoctor, IsTWSTRSDoctor },
            { QuestionnaireType.TWSTRSPatient, IsTWSTRSPatient }
            };

            foreach (var questionnaire in questionnairesToCreate)
            {
                if (questionnaire.Value)
                {
                    var dto = new CreateQuestionnaireRecordsDto
                    {
                        VisitId = Result.VisitId,
                        QuestionnaireType = questionnaire.Key,
                    };
                    var result = await HttpClientService.PostAsyncWithResponseMessage($"api/Questionnaires/{dto.VisitId}/{(int) questionnaire.Key}", dto);
                }
            }
        }
        public async Task AddQuestionnairesForOnline()
        {
            var questionnairesToCreate = new Dictionary<QuestionnaireType, bool>
            {
                { QuestionnaireType.CGIDoctor, IsCGIDoctor },
                { QuestionnaireType.CGIPatient, IsCGIPatient },
                { QuestionnaireType.TWSTRSPatient, IsTWSTRSPatientOnline }
            };

            foreach (var questionnaire in questionnairesToCreate)
            {
                if (questionnaire.Value)
                {
                    var dto = new CreateQuestionnaireRecordsDto
                    {
                        VisitId = Result.VisitId,
                        QuestionnaireType = questionnaire.Key,
                    };
                    var result = await HttpClientService.PostAsyncWithResponseMessage($"api/Questionnaires/{dto.VisitId}/{(int) questionnaire.Key}", dto);
                }
            }
        }
    }
}
