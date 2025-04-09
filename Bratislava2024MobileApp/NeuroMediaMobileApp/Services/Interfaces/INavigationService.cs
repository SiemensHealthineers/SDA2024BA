using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroMediaMobileApp.Services.Interfaces
{
    public interface INavigationService
    {
        Task NavigateToAsync(string pageName);
        Task DisplayNavigationErrorAlertAsync(string title, string message, string cancel);
    }
}
