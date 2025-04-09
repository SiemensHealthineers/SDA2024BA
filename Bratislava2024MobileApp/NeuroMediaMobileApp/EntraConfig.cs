using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroMediaMobileApp
{
    public static class EntraConfig
    {
        public static string Authority = "https://turituri866.ciamlogin.com/";
        public static string ClientId = "8a9ba43a-5b47-426e-a500-990dffc1abad";
        public static string TenantId = "72cfca24-64f2-4fa3-b955-cc3559aa0501";
        public static string[] Scopes = ["https://turituri866.onmicrosoft.com/30e5a9c9-b6fd-47e2-bdc2-b755aa3b222c/access_as_user"];
        public static object? ParentWindow { get; set; }

    }
}
