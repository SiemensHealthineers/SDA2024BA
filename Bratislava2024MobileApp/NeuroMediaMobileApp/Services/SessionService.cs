using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.ApplicationModel.Communication;

using NeuroMediaMobileApp.Models.DTOs;
using NeuroMediaMobileApp.Models.Enums;
using NeuroMediaMobileApp.Services.Interfaces;

namespace NeuroMediaMobileApp.Services
{
    public class SessionService : ISessionService
    {
        private static string s_userName = default!;
        private static string s_email = default!;
        private static string s_roles = default!;
        private static string s_oid = default!;
        private static string s_tenant = default!;
        private static string s_accessToken = default!;
        private static string s_idToken = default!;
        private static QuestionnaireRecordDto s_questionnaireRecord = default!;
        private static int s_currentPatientId = default!;
        public void CreateSession(string userName, string email, string roles, string oid, string tenant, string accessToken, string idToken)
        {
            s_userName = userName;
            s_email = email;
            s_roles = roles;
            s_oid = oid;
            s_tenant = tenant;
            s_accessToken = accessToken;
            s_idToken = idToken;
        }

        public void ClearSession()
        {
            s_userName = string.Empty;
            s_email = string.Empty;
            s_roles = string.Empty;
            s_oid = string.Empty;
            s_tenant = string.Empty;
            s_questionnaireRecord = null;
            s_currentPatientId = 0;
        }

        public string UserName => s_userName;

        public string Email => s_email;

        public string Roles => s_roles;

        public string Oid => s_oid;

        public string Tenant => s_tenant;
        public string AccessToken => s_accessToken;
        public string IdToken => s_idToken;

        public QuestionnaireRecordDto QuestionnaireRecord
        {
            get => s_questionnaireRecord;
            set => s_questionnaireRecord = value;
        }
        public int CurrentPatientId
        {
            get => s_currentPatientId;
            set => s_currentPatientId = value;
        }
    }
}
