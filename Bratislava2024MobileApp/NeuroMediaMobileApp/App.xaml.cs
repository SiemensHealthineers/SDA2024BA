using NeuroMediaMobileApp.Helpers;
using Microsoft.Maui.Controls;


namespace NeuroMediaMobileApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
