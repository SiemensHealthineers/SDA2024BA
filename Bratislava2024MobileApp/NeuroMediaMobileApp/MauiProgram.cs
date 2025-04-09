using Microsoft.Extensions.Logging;

using NeuroMediaMobileApp.ViewModel;
using NeuroMediaMobileApp.View;
using NeuroMediaMobileApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using NeuroMediaMobileApp.Services.Interfaces;
using NeuroMediaMobileApp.Controls;

namespace NeuroMediaMobileApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("MaterialIcons-Regular.ttf", "GoogleMaterialFont");
                });

            var uri = "https://neuromedia-api.azurewebsites.net/api/";
            builder.Services.AddHttpClient("ServerAPI", client => client.BaseAddress = new Uri(uri));

            builder.Services.AddSingleton<IHttpClientService>(sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var client = httpClientFactory.CreateClient("ServerAPI");
                return new HttpClientService(client);
            });

            builder.Services.AddSingleton<IPatientService, PatientService>();
            builder.Services.AddSingleton<IVisitService, VisitService>();
            builder.Services.AddSingleton<IVideoService, VideoService>();
            builder.Services.AddSingleton<IQuestionnaireService, QuestionnaireService>();
            builder.Services.AddSingleton<IAlertService, AlertService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<ISessionService, SessionService>();
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddTransient<QuestionnairePage>();
            builder.Services.AddTransient<QuestionnaireViewModel>();

            builder.Services.AddTransient<PersonalInformation>();
            builder.Services.AddTransient<MedicalInformation>();

            builder.Services.AddTransient<PatientProfileDetailViewModel>();

            builder.Services.AddTransient<PatientProfileViewModel>();
            builder.Services.AddTransient<PatientProfilePage>();
            builder.Services.AddTransient<PatientProfileDetailPage>();

            builder.Services.AddTransient<PendingTasksViewModel>();

            builder.Services.AddTransient<ListOfPatientsPage>();
            builder.Services.AddTransient<ListOfPatientsViewModel>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();
            ServiceHelper.Services = app.Services;

            return builder.Build();
        }
    }
}
