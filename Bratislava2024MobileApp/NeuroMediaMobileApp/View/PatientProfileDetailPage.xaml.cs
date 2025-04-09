using NeuroMediaMobileApp.ViewModel;
using NeuroMediaMobileApp.Controls;
namespace NeuroMediaMobileApp.View;

public partial class PatientProfileDetailPage : ContentPage
{
    public PatientProfileDetailPage(PatientProfileDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        viewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(PatientProfileDetailViewModel.Patient))
            {
                PersonalInformation.BindingContext = viewModel.Patient;
                MedicalInformation.BindingContext = viewModel.Patient;
            }
            else if(e.PropertyName == nameof(PatientProfileDetailViewModel.Visits))
            {
                PatientVisits.BindingContext = viewModel.Visits;
            }
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((PatientProfileDetailViewModel) BindingContext).LoadPatientDataAsync();
        await ((PatientProfileDetailViewModel) BindingContext).LoadListOfVisitsAsync();
    }

}
