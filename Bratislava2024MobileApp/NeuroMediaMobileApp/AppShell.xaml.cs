using NeuroMediaMobileApp.View;
namespace NeuroMediaMobileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(PatientProfilePage), typeof(PatientProfilePage));
            Routing.RegisterRoute(nameof(QuestionnairePage), typeof(QuestionnairePage));
            Routing.RegisterRoute(nameof(PatientProfileDetailPage), typeof(PatientProfileDetailPage));
            Routing.RegisterRoute(nameof(ListOfPatientsPage), typeof(ListOfPatientsPage));
        }
    }
}
