using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMediaMobileApp.Services.Interfaces;

namespace NeuroMediaMobileApp.Services
{
    internal class AlertService : IAlertService
    {
        Task IAlertService.DisplayAlertAsync(string title, string message, string cancel)
        {
            return Shell.Current.DisplayAlert(title, message, cancel);
        }
    }
}
