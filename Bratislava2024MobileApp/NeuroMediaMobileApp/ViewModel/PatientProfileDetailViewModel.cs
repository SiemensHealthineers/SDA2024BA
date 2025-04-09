using System.Threading.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Maui.Graphics;

using NeuroMediaMobileApp.Models.Entities;
using NeuroMediaMobileApp.Services.Interfaces;

using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace NeuroMediaMobileApp.ViewModel
{
    public partial class PatientProfileDetailViewModel : ObservableObject
    {
        private readonly IPatientService _patientService = default;
        private readonly IVisitService _visitService = default;
        private readonly ISessionService _sessionService = default;

        public ObservableCollection<Visit> Visits { get; } = new ObservableCollection<Visit>();
        [ObservableProperty]
        public Patient patient;

        public PatientProfileDetailViewModel(IPatientService patientService, IVisitService visitService, ISessionService sessionService)
        {
            _patientService = patientService;
            _visitService = visitService;
            _sessionService = sessionService;

        }

        [RelayCommand]
        public async Task LoadPatientDataAsync()
        {
            var patient = await _patientService.GetPatientDataAsync(_sessionService.CurrentPatientId);
            Console.WriteLine($"Start");

            if (patient != null)
            {
                Patient = patient;
                Console.WriteLine($"Patient name: {Patient.Name}");
            }
            else
            {
                Console.WriteLine("No patient data found.");
            }
        }

        [RelayCommand]
        public async Task LoadListOfVisitsAsync()
        {
            var visits = await _visitService.GetAllVisitsDataAsync(_sessionService.CurrentPatientId);
            Visits.Clear();
            var sortedVisits = visits.OrderByDescending(visit => visit.DateOfVisit);


            foreach (var visit in sortedVisits)
            {
                Visits.Add(visit);
            }
        }
    }
}
