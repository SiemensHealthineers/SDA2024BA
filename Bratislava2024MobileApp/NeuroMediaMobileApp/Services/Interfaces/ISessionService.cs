using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using NeuroMediaMobileApp.Models.DTOs;
using NeuroMediaMobileApp.Models.Enums;

namespace NeuroMediaMobileApp.Services.Interfaces
{
    public interface ISessionService
    {
        public void CreateSession(string userName, string email, string roles, string oid, string tenant, string accesToken, string idToken);
        public void ClearSession();

        public string UserName { get; }
        public string Email { get; }
        public string Roles { get; }
        public string Oid { get; }
        public string Tenant { get; }
        public string AccessToken { get; }
        public string IdToken { get; }
        QuestionnaireRecordDto QuestionnaireRecord { get; set; }
        public int CurrentPatientId { get; set; }
    }
}
