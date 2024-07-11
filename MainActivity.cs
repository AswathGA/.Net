
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Com.Uxcam.Datamodel;
using Com.UXCam;
using Rg.Plugins.Popup.Extensions;
using Stormlion.ImageCropper.Droid;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Draftsy.Droid
{
    [Activity(Label = "Draftsy", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        [System.Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            UXCam.OccludeSensitiveScreen(true);
            UXConfig.Builder builder = new UXConfig.Builder("10gwnpywbypuyx1");
            builder.EnableAutomaticScreenNameTagging(false);
            builder.EnableImprovedScreenCapture(false);

            UXConfig configuration = new UXConfig(builder);
            UXCam.StartWithConfiguration(configuration);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Rg.Plugins.Popup.Popup.Init(this);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
            GoogleVisionBarCodeScanner.Droid.RendererInitializer.Init();
            Platform.Init();
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;

            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);

            Xamarin.FormsMaps.Init(this, savedInstanceState);

            /*if (Intent.Extras != null)
            {
                CreateNotificationFromIntent(Intent);
            }*/

            LoadApplication(new App());
            Window.SetSoftInputMode(SoftInput.AdjustResize);
            //Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }

        protected override void OnNewIntent(Intent intent)
        {
            /*CreateNotificationFromIntent(Intent);
            base.OnNewIntent(intent);
            Intent = intent;*/
        }

        void CreateNotificationFromIntent(Intent intent)
        {
            /*if (intent?.Extras != null)
            {
                App.notificationType = intent.Extras.GetString(MyFirebaseMessagingService.Type);
                
                if(App.notificationType == "draft")
                {
                    App.notificationLeagueId = intent.Extras.GetString(MyFirebaseMessagingService.LeagueId);
                    App.notificationLeagueDetail = intent.Extras.GetString(MyFirebaseMessagingService.LeagueDetail);
                }
            }*/
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            Platform.OnActivityResult(requestCode, resultCode, data);
        }

        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                App.Current.MainPage.Navigation.PopPopupAsync(true);
            }
        }
    }
}