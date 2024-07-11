using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Extensions;
using Android.OS;
using Firebase.Messaging;
using System.Threading.Tasks;

namespace Draftsy.Droid
{

    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _ = setTokenAsync();

            // Create your application here
            var mainIntent = new Intent(Application.Context, typeof(MainActivity));

            if (Intent.Extras != null)
            {
                mainIntent.PutExtras(Intent.Extras);
            }
            mainIntent.SetFlags(ActivityFlags.SingleTop);

            CreateNotificationFromIntent(Intent);

            StartActivity(mainIntent);
        }

        public async Task setTokenAsync()
        {
            var token = await FirebaseMessaging.Instance.GetToken();
            App.deviceToken = token.ToString();
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            Intent = intent;
            CreateNotificationFromIntent(Intent);
        }
        void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                /* string title = intent.Extras.GetString(AndroidNotificationManager.TitleKey);
                 string message = intent.Extras.GetString(AndroidNotificationManager.MessageKey);
                 string memberInvitationId = intent.Extras.GetString(AndroidNotificationManager.MemberInvitationId);
                 string type = intent.Extras.GetString(AndroidNotificationManager.pushType);*/

                //DependencyService.Get<INotificationManager>().ReceiveNotification(title, message, memberNotificationId: memberInvitationId, pushType: type);
                //App.notificationType = "Game Today";
            }
        }
    }
}