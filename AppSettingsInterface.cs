using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Draftsy.dependency;
using Draftsy.Droid.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppSettingsInterface))]
namespace Draftsy.Droid.Dependency
{
    public class AppSettingsInterface : IAppSettingsHelper
    {
        public void OpenAppSettings()
        {
            var intent = new Intent(Android.Provider.Settings.ActionApplicationDetailsSettings);
            intent.AddFlags(ActivityFlags.NewTask);
            string package_name = "com.draftsy";
            var uri = Android.Net.Uri.FromParts("package", package_name, null);
            intent.SetData(uri);
            Android.App.Application.Context.StartActivity(intent);
        }
    }
}