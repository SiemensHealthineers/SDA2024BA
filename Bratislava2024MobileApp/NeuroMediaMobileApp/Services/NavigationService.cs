using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMediaMobileApp.Services.Interfaces;

namespace NeuroMediaMobileApp.Services
{
    public class NavigationService : INavigationService
    {
        public Task NavigateToAsync(string pageName)
        {
            return Shell.Current.GoToAsync(pageName);
        }

        public Task DisplayNavigationErrorAlertAsync(string title, string message, string cancel)
        {
            return Shell.Current.DisplayAlert(title, message, cancel);
        }
    }
}
