using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroMediaMobileApp.Services.Interfaces;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Identity.Client;
using NeuroMediaMobileApp.View;
using NeuroMediaMobileApp.Models.Entities;

namespace NeuroMediaMobileApp.ViewModel
{
    public partial class PatientProfileViewModel : ObservableObject
    {
        private readonly ISessionService _sessionService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IPatientService _patientService = default;
        public PendingTasksViewModel PendingTasksViewModel { get; }

        [ObservableProperty]
        public Patient patient;
        public IAsyncRelayCommand OnLogoutCommand { get; }

        public PatientProfileViewModel(ISessionService sessionService, IAuthenticationService authenticationService, IPatientService patientService, PendingTasksViewModel pendingTasksViewModel)
        {
            _sessionService = sessionService;
            _authenticationService = authenticationService;
            _patientService = patientService;
            OnLogoutCommand = new AsyncRelayCommand(OnLogout);
            PendingTasksViewModel = pendingTasksViewModel;
        }

        public async Task OnLogout()
        {
            await _authenticationService.Logout();
        }
        [RelayCommand]
        public async Task LoadPatientDataAsync()
        {
            var patient = await _patientService.GetPatientDataAsync(_sessionService.CurrentPatientId);
            Patient = patient;
            await PendingTasksViewModel.LoadPendingTasksAsync(patient.Id);
        }
        [RelayCommand]
        private async Task NavigateToProfileDetailPage()
        {
            await Shell.Current.GoToAsync(nameof(PatientProfileDetailPage));
        }
    }
}
