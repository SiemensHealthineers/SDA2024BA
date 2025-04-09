using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;

using NeuroMedia.Application.Features.Patients.Queries.GetPatientById;
using NeuroMedia.Application.Features.Visits.Queries;

using static System.Net.WebRequestMethods;
using NeuroMedia.WebUI.Services.Interfaces;

namespace NeuroMedia.WebUI.Pages
{
    public partial class PatientDetail
    {
        [Parameter]
        public GetPatientByIdDto? Patient { get; set; }
        [Parameter]
        public List<GetAllVisitsDto> VisitsList { get; set; }
        [Inject]
        public IHttpClientService HttpClientService { get; set; } = default!;

        private async Task LoadVisits()
        {
            VisitsList = await HttpClientService.GetListAsync<GetAllVisitsDto>($"api/PatientsVisits/{Patient.Id}");
            StateHasChanged();
        }

        private async Task OnClose()
        {
            await JSRuntime.InvokeVoidAsync("closeModal", "patientDetailModal");
        }

        private async Task NavigateToVisitDetails(int patientId, int visitId)
        {
            Navigation.NavigateTo($"visit/personal/{patientId}/{visitId}");
            await OnClose();
        }

        private async Task HandleNewVisit()
        {
            await LoadVisits();
        }
        private async Task CreateNewVisitModal()
        {
            if (JSRuntime == null)
            {
                throw new InvalidOperationException("JSRuntime is not initialized.");
            }
            await JSRuntime.InvokeVoidAsync("ShowModal", "addVisitModal");
        }
    }
}
