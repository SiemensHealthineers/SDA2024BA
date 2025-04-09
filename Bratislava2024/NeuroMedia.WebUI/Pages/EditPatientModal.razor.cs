using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using NeuroMedia.Application.Features.Patients.Commands.UpdatePatient;
using NeuroMedia.Application.Features.Patients.Queries.GetPatientById;
using NeuroMedia.WebUI.Models;
using NeuroMedia.WebUI.Services.Interfaces;

using System.Net.Http.Json;

using static System.Net.WebRequestMethods;

namespace NeuroMedia.WebUI.Pages
{
    public partial class EditPatientModal
    {
        [Parameter]
        public GetPatientByIdDto? InputPatient { get; set; }
        [Parameter] public EventCallback<PatientDataTransfer> OnPatientEdited { get; set; }
        private PatientDataTransfer? patient;
        [Inject]
        public ILogger<EditPatientModal> Logger { get; set; }
        [Inject]
        private IHttpClientService HttpClientService { get; set; }
        [Inject]
        private NavigationManager Navigation {  get; set; }
        [Inject]
        private IJSRuntime JSRuntime {  get; set; }

        public string EmailNotUnique { get; set; } = string.Empty;
        public string DateOfBirthInTheFuture { get; set; } = string.Empty;
        public string DateOfDiagnosisInTheFuture { get; set; } = string.Empty;


        public PatientDataTransfer ExtractPatientData(GetPatientByIdDto inputPatient)
        {
            return new PatientDataTransfer
            {
                Name = inputPatient.Name,
                Surname = inputPatient.Surname,
                DateOfBirth = DateTime.SpecifyKind(inputPatient.DateOfBirth, DateTimeKind.Utc),
                Sex = inputPatient.Sex,
                Diagnosis = inputPatient.Diagnosis,
                Email = inputPatient.Email,
                PhoneNumber = inputPatient.PhoneNumber,
                HighestEducation = inputPatient.HighestEducation,
                EmploymentStatus = inputPatient.EmploymentStatus,
                RapExamination = inputPatient.RapExamination,
                PreviousBotulinumToxinApplication = inputPatient.PreviousBotulinumToxinApplication,
                IsActive = inputPatient.IsActive,
                DateOfDiagnosis = DateTime.SpecifyKind(inputPatient.DateOfDiagnosis, DateTimeKind.Utc)
            };
        }


        protected override void OnParametersSet()
        {
            if (InputPatient != null)
            {
                patient = ExtractPatientData(InputPatient);
            }
        }


        private void HandleValidSubmit()
        {
            if (patient != null)
            {
                UpdatePatient(patient, InputPatient.Id);
                EmailNotUnique = string.Empty;
            }
            else
            {
                throw new NullReferenceException();
            }
        }


        private async Task UpdatePatient(PatientDataTransfer inputPatient, int patientId)
        {
            DateOfBirthInTheFuture = string.Empty;
            DateOfDiagnosisInTheFuture = string.Empty;
            EmailNotUnique = string.Empty;

            var updatedPatient = new UpdatePatientDto
            {
                Name = inputPatient.Name ?? string.Empty,
                Surname = inputPatient.Surname ?? string.Empty,
                DateOfBirth = DateTime.SpecifyKind(inputPatient.DateOfBirth, DateTimeKind.Utc),
                Sex = inputPatient.Sex,
                Diagnosis = inputPatient.Diagnosis,
                Email = inputPatient?.Email ?? string.Empty,
                IsActive = inputPatient.IsActive,
                DateOfDiagnosis = DateTime.SpecifyKind(inputPatient.DateOfDiagnosis, DateTimeKind.Utc),
                PhoneNumber = inputPatient.PhoneNumber,
                RapExamination = inputPatient.RapExamination,
                PreviousBotulinumToxinApplication = inputPatient.PreviousBotulinumToxinApplication,
                EmploymentStatus = inputPatient.EmploymentStatus,
                HighestEducation = inputPatient.HighestEducation
            };

            var response = await HttpClientService.PutAsyncWithResponseMessage($"api/Patients/{patientId}", updatedPatient);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                await OnPatientEdited.InvokeAsync(inputPatient);
                await JSRuntime.InvokeVoidAsync("closeModal", "editPatientModal");
            }
            else
            {
                Logger.LogError("Failed to create a patient.");

                if (responseBody.Contains("mail"))
                {
                    EmailNotUnique = "This email is already used.";
                    Logger.LogWarning("Attempted to use an already existing email {Email}", updatedPatient.Email);
                }

                if (responseBody.Contains("The DateOfBirth field cannot be in the future."))
                {
                    DateOfBirthInTheFuture = "The date of birth cannot be in the future.";
                    Logger.LogWarning("The DateOfBirth field cannot be in the future.");
                }

                if (responseBody.Contains("The DateOfDiagnosis field cannot be in the future."))
                {
                    DateOfDiagnosisInTheFuture = "The date of diagnosis cannot be in the future.";
                    Logger.LogWarning("The DateOfDiagnosis field cannot be in the future.");
                }
            }
            StateHasChanged();
        }

        private async Task OnClose()
        {
            await JSRuntime.InvokeVoidAsync("closeModal", "editPatientModal");
            EmailNotUnique = string.Empty;
        }
        private async Task ShowConfirmationModal()
        {
            await JSRuntime.InvokeVoidAsync("ShowModal", "deactivateModal");
        }
    }
}
