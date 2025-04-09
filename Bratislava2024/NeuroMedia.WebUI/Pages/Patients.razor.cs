using Microsoft.AspNetCore.Components;

using System.Net.Http.Json;

using static System.Net.Mime.MediaTypeNames;

using NeuroMedia.Application.Features.Patients.Queries.GetAllPatients;
using NeuroMedia.Application.Features.Patients.Queries.GetPatientById;
using NeuroMedia.Application.Features.Visits.Queries;
using Microsoft.JSInterop;

using NeuroMedia.WebUI.Models;
using NeuroMedia.WebUI.Services.Interfaces;

namespace NeuroMedia.WebUI.Pages
{
    public partial class Patients : ComponentBase
    {
        [Inject]
        public IHttpClientService HttpClientService { get; set; } = default!;
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public List<GetAllPatientsDto>? PatientsList { get; private set; } = new List<GetAllPatientsDto>();
        private string swapGroup;
        public bool ActiveState { get; set; }

        private GetPatientByIdDto? SelectedPatient;
        private int SelectedPatientId;
        private List<GetAllVisitsDto>? VisitsList;

        private async Task LoadPatients()
        {
            PatientsList = await HttpClientService.GetListAsync<GetAllPatientsDto>("api/patients");
            StateHasChanged();
        }


        private async Task CreateNewPatientModal()
        {
            if (JSRuntime == null)
            {
                throw new InvalidOperationException("JSRuntime is not initialized.");
            }
            await JSRuntime.InvokeVoidAsync("ShowModal", "addPatientModal");
        }


        private async Task ShowDetailsModal(int patientId)
        {
            SelectedPatient = await GetPatientDetailsById(patientId, ActiveState);
            SelectedPatientId = SelectedPatient.Id;
            VisitsList = await GetListOfVisitsByPatientId(patientId);

            if (JSRuntime == null)
            {
                throw new InvalidOperationException("JSRuntime is not initialized.");
            }
            await JSRuntime.InvokeVoidAsync("ShowModal", "patientDetailModal");
        }


        private async Task ShowEditPatientModal(int patientId)
        {
            SelectedPatient = await GetPatientDetailsById(patientId, ActiveState);
            SelectedPatientId = patientId;
            StateHasChanged();

            if (JSRuntime == null)
            {
                throw new InvalidOperationException("JSRuntime is not initialized.");
            }
            await JSRuntime.InvokeVoidAsync("ShowModal", "editPatientModal");
        }


        protected override async Task OnInitializedAsync()
        {
            PatientsList = await HttpClientService.GetListAsync<GetAllPatientsDto>("api/patients");

            SetPatientsGroup();
        }


        public async Task GetPatientsOnToggleClick()
        {
            PatientsList = await GetAllStatusMatchingPatients(ActiveState);

            SetPatientsGroup();

            StateHasChanged();
        }

        public async Task<List<GetAllPatientsDto>> GetAllStatusMatchingPatients(bool areActive)
        {
            if (areActive)
            {
                return await HttpClientService.GetListAsync<GetAllPatientsDto>("api/patients/deactivated");
            }
            else
            {
                return await HttpClientService.GetListAsync<GetAllPatientsDto>("api/patients");
            }
        }

        public void SetPatientsGroup()
        {

            if (PatientsList != null)
            {
                if (!ActiveState)
                {
                    swapGroup = "Deactivated";
                    ActiveState = true;
                }
                else
                {
                    swapGroup = "Active";
                    ActiveState = false;
                }
            }

        }

        public static string GetActualPatientGroup(bool state)
        {
            return state ? "Active" : "Deactivated";
        }

        public async Task<GetPatientByIdDto> GetPatientDetailsById(int patientId, bool areActive)
        {
            if (areActive)
            {
                return await HttpClientService.GetAsync<GetPatientByIdDto>($"api/patients/{patientId}");

            }
            else
            {
                return await HttpClientService.GetAsync<GetPatientByIdDto>($"api/patients/deactivated/{patientId}");
            }
        }


        public async Task<List<GetAllVisitsDto>> GetListOfVisitsByPatientId(int patientId)
        {
            return await HttpClientService.GetListAsync<GetAllVisitsDto>($"api/PatientsVisits/{patientId}");
        }

        private async Task HandleNewPatient(PatientDataTransfer patient)
        {
            await LoadPatients();
        }

        private async Task HandleEditedPatient(PatientDataTransfer editedPatient)
        {
            await LoadPatients();

            if (SelectedPatientId != 0)
            {
                SelectedPatient = await GetPatientDetailsById(SelectedPatientId, ActiveState);
                VisitsList = await GetListOfVisitsByPatientId(SelectedPatientId);
                StateHasChanged();
            }
        }

        private async Task HandlePatientDeactivated(int patientId)
        {
            if (ActiveState)
            {
                PatientsList.RemoveAll(p => p.Id == patientId);
            }
            else
            {
                var deactivatedPatient = await GetPatientDetailsById(patientId, false);
                PatientsList.Add(new GetAllPatientsDto
                {
                    Id = deactivatedPatient.Id,
                    Name = deactivatedPatient.Name,
                    Surname = deactivatedPatient.Surname,
                    Email = deactivatedPatient.Email,
                    DateOfBirth = deactivatedPatient.DateOfBirth,
                    Sex = deactivatedPatient.Sex,
                    Diagnosis = deactivatedPatient.Diagnosis
                });
            }

            await JSRuntime.InvokeVoidAsync("closeModal", "deactivateModal");
            await JSRuntime.InvokeVoidAsync("closeModal", "patientDetailModal");
            await JSRuntime.InvokeVoidAsync("closeModal", "editPatientModal");

            StateHasChanged();
        }

        private async Task HandlePatientReactivated(int patientId)
        {
            if (ActiveState)
            {
                var reactivatedPatient = await GetPatientDetailsById(patientId, true);
                PatientsList.Add(new GetAllPatientsDto
                {
                    Id = reactivatedPatient.Id,
                    Name = reactivatedPatient.Name,
                    Surname = reactivatedPatient.Surname,
                    Email = reactivatedPatient.Email,
                    DateOfBirth = reactivatedPatient.DateOfBirth,
                    Sex = reactivatedPatient.Sex,
                    Diagnosis = reactivatedPatient.Diagnosis
                });
            }
            else
            {
                PatientsList.RemoveAll(p => p.Id == patientId);
            }

            await JSRuntime.InvokeVoidAsync("closeModal", "reactivateModal");
            await JSRuntime.InvokeVoidAsync("closeModal", "patientDetailModal");

            StateHasChanged();
        }

        private async Task HandlePatientDeleted(int patientId)
        {
            if (!ActiveState)
            {
                PatientsList.RemoveAll(p => p.Id == patientId);
            }

            await JSRuntime.InvokeVoidAsync("closeModal", "deleteModal");
            await JSRuntime.InvokeVoidAsync("closeModal", "patientDetailModal");

            StateHasChanged();
        }
    }
}
