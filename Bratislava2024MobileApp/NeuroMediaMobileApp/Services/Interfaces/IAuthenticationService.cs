using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroMediaMobileApp.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task Login();
        Task Logout();
        Task CheckIfLoggedIn();
        bool IsPrivileged(string userRoles);
        bool IsPatient(string userRoles);
    }
    public record AuthenticationToken(string DisplayName, DateTimeOffset ExpiresOn, string AccessToken, string IdToken, string UserId);
}
