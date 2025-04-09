using Microsoft.Maui.Controls;
using NeuroMediaMobileApp.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace NeuroMediaMobileApp.Controls
{
    public partial class PendingTasks : ContentView
    {
        public PendingTasks()
        {
            InitializeComponent();
        }

        public PendingTasks(PendingTasksViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
