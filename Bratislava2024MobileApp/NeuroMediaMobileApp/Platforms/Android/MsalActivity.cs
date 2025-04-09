using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;

using Microsoft.Identity.Client;

namespace NeuroMediaMobileApp.Platforms.Android
{
    [Activity(Exported = true)]
    [IntentFilter([Intent.ActionView],
        Categories = [Intent.CategoryBrowsable, Intent.CategoryDefault],
        DataHost = "auth",
        DataScheme = "msal8a9ba43a-5b47-426e-a500-990dffc1abad")]
    public class MsalActivity : BrowserTabActivity
    {
    }
}
