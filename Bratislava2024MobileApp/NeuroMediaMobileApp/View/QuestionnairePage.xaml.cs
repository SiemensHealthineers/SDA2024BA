using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;

using NeuroMediaMobileApp.Models;
using NeuroMediaMobileApp.Models.DTOs;
using NeuroMediaMobileApp.ViewModel;

namespace NeuroMediaMobileApp.View
{
    public partial class QuestionnairePage : ContentPage
    {
        public QuestionnairePage(QuestionnaireViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ((QuestionnaireViewModel) BindingContext).LoadQuestions();
        }
    }
}
