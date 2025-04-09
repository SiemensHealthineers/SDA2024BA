using NeuroMediaMobileApp.ViewModel;

namespace NeuroMediaMobileApp.View;

public partial class PatientProfilePage : ContentPage
{
    public PatientProfilePage(PatientProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((PatientProfileViewModel) BindingContext).LoadPatientDataAsync();
    }
}
