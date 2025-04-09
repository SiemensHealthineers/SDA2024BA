using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using NeuroMediaMobileApp.Models.Entities;
using NeuroMediaMobileApp.Services.Interfaces;
using NeuroMediaMobileApp.View;

namespace NeuroMediaMobileApp.ViewModel
{
    public partial class ListOfPatientsViewModel : ObservableObject
    {
        public IAsyncRelayCommand OnLogoutCommand { get; }
        private readonly IPatientService _patientService = default;
        private readonly ISessionService _sessionService = default;
        private readonly IAuthenticationService _authenticationService;

        public ListOfPatientsViewModel(IPatientService patientService, ISessionService sessionService, IAuthenticationService authenticationService)
        {
            _patientService = patientService;
            _sessionService = sessionService;
            _authenticationService = authenticationService;
            OnLogoutCommand = new AsyncRelayCommand(OnLogout);
        }

        public ObservableCollection<Patient> Patients { get; } = new ObservableCollection<Patient>();

        public async Task OnLogout()
        {
            await _authenticationService.Logout();
        }

        [RelayCommand]
        public async Task LoadListOfPatientsDataAsync()
        {
            var patients = await _patientService.GetAllPatientsDataAsync();
            Patients.Clear();
            foreach (var patient in patients)
            {
                Patients.Add(patient);
            }
        }
        [RelayCommand]
        public async Task LoadPatientDataAsync(int id)
        {
            _sessionService.CurrentPatientId = id;
            await Shell.Current.GoToAsync(nameof(PatientProfilePage));
        }
    }
}
