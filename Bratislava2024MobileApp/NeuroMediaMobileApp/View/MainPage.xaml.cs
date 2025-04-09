using NeuroMediaMobileApp.ViewModel;

namespace NeuroMediaMobileApp.View;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
