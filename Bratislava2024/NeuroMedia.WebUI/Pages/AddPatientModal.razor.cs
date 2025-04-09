using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using NeuroMedia.Application.Features.Patients.Commands.CreatePatient;
using NeuroMedia.WebUI.Models;
using NeuroMedia.WebUI.Services.Interfaces;

using System.Net.Http.Json;

namespace NeuroMedia.WebUI.Pages
{
    public partial class AddPatientModal
    {
        [Parameter] public EventCallback<PatientDataTransfer> OnPatientAdded { get; set; }
        public PatientDataTransfer Patient { get; set; } = new PatientDataTransfer();
        [Inject]
        private IHttpClientService HttpClientService { get; set; } = default!;

        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }
        [Inject]
        public ILogger<AddPatientModal> Logger { get; set; }

        public string EmailNotUnique { get; set; } = string.Empty;
        public string DateOfBirthInTheFuture { get; set; } = string.Empty;
        public string DateOfDiagnosisInTheFuture { get; set; } = string.Empty;
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Patient.DateOfDiagnosis = DateTime.Today;
            Patient.DateOfBirth = DateTime.Today;
        }

        private async Task HandleValidSubmit()
        {
            DateOfBirthInTheFuture = string.Empty;
            DateOfDiagnosisInTheFuture = string.Empty;
            EmailNotUnique = string.Empty;

            var dto = new CreatePatientDto
            {
                Name = Patient.Name,
                Surname = Patient.Surname,
                DateOfBirth = DateTime.SpecifyKind(Patient.DateOfBirth, DateTimeKind.Utc),
                Sex = Patient.Sex,
                Email = Patient.Email,
                PhoneNumber = Patient.PhoneNumber,
                Diagnosis = Patient.Diagnosis,
                IsActive = true,
                RapExamination = Patient.RapExamination,
                PreviousBotulinumToxinApplication = Patient.PreviousBotulinumToxinApplication,
                HighestEducation = Patient.HighestEducation,
                EmploymentStatus = Patient.EmploymentStatus,
                DateOfDiagnosis = DateTime.SpecifyKind(Patient.DateOfDiagnosis, DateTimeKind.Utc)
            };

            var response = await HttpClientService.PostAsyncWithResponseMessage("api/Patients", dto);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                await OnPatientAdded.InvokeAsync(Patient);
                Patient = new PatientDataTransfer
                {
                    DateOfDiagnosis = DateTime.Today,
                    DateOfBirth = DateTime.Today
                };
                await JS.InvokeVoidAsync("closeModal", "addPatientModal");
            }
            else
            {
                Logger.LogError("Failed to create a patient.");

                if (responseBody.Contains("mail"))
                {
                    EmailNotUnique = "This email is already used.";
                    Logger.LogWarning("Attempted to use an already existing email {Email}", Patient.Email);
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
    }
}
