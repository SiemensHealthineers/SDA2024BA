using System.Xml;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using NeuroMediaMobileApp.Services.Interfaces;
using Microsoft.Identity.Client;

using NeuroMediaMobileApp.View;
using System.IdentityModel.Tokens.Jwt;
using NeuroMediaMobileApp.Services;

namespace NeuroMediaMobileApp.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ISessionService _sessionService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IAlertService _alertService;
        public IAsyncRelayCommand OnLoginCommand { get; }

        public MainViewModel(ISessionService sessionService, IAuthenticationService authenticationService, IAlertService alertService)
        {
            _sessionService = sessionService;
            _authenticationService = authenticationService;
            _alertService = alertService;
            OnLoginCommand = new AsyncRelayCommand(OnLogin);
            AutoLogin();
        }
        public async Task AutoLogin()
        {
            try
            {
                await _authenticationService.CheckIfLoggedIn();

                if (_authenticationService.IsPrivileged(_sessionService.Roles))
                {
                    await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync(nameof(ListOfPatientsPage)));
                }
                else if (_authenticationService.IsPatient(_sessionService.Roles))
                {
                    _sessionService.CurrentPatientId = 1;
                    await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync(nameof(PatientProfilePage)));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Auto login failed: {ex.Message}");
            }
        }
        public async Task OnLogin()
        {
            try
            {
                await _authenticationService.Login();

                if (_authenticationService.IsPrivileged(_sessionService.Roles))
                {
                    await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync(nameof(ListOfPatientsPage)));
                }
                else if (_authenticationService.IsPatient(_sessionService.Roles))
                {
                    _sessionService.CurrentPatientId = 1;
                    await MainThread.InvokeOnMainThreadAsync(async () => await Shell.Current.GoToAsync(nameof(PatientProfilePage)));
                }
                else
                {
                    await _alertService.DisplayAlertAsync(
                        "Invalid Account",
                        "Your account does not have any valid assigned role. Please try again with different account or contact an administrator of this service.",
                        "Ok");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
            }
        }
    }
}
