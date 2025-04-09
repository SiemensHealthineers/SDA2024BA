using NeuroMediaMobileApp.ViewModel;
namespace NeuroMediaMobileApp.View;

public partial class ListOfPatientsPage : ContentPage
{
	public ListOfPatientsPage(ListOfPatientsViewModel viewModel)
	{
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ((ListOfPatientsViewModel) BindingContext).LoadListOfPatientsDataAsync();
    }
}
