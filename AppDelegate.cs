using System;
using Draftsy.iOS.Dependency;
using Foundation;
using PushKit;
using Stormlion.ImageCropper.iOS;
using UIKit;
using UserNotifications;
using Firebase.CloudMessaging;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Plugin.LocalNotification;
using Xamarin.Forms;
using Draftsy.utils;
using ColorPicker.iOS;


namespace Draftsy.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();


            //UXCam.OptIntoSchematicRecordings();
           // //UXCamConfiguration configuration = new UXCamConfiguration(userAppKey: "10gwnpywbypuyx1");
           // configuration.EnableAutomaticScreenNameTagging = false;
           // configuration.EnableAdvancedGestureRecognition = false;
            //UXCam.StartWithConfiguration(configuration);

            Rg.Plugins.Popup.Popup.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            Platform.Init();

            App.ScreenHeight = (int)UIScreen.MainScreen.Bounds.Height;
            App.ScreenWidth = (int)UIScreen.MainScreen.Bounds.Width;

            GoogleVisionBarCodeScanner.iOS.Initializer.Init();
            Firebase.Core.App.Configure();
            ColorPickerEffects.Init();

            Xamarin.FormsMaps.Init();

            Xamarin.IQKeyboardManager.SharedManager.EnableAutoToolbar = true;
            Xamarin.IQKeyboardManager.SharedManager.ShouldResignOnTouchOutside = true;
            Xamarin.IQKeyboardManager.SharedManager.ShouldToolbarUsesTextFieldTintColor = true;
            Xamarin.IQKeyboardManager.SharedManager.KeyboardDistanceFromTextField = 300f;

            Messaging.SharedInstance.Delegate = this;

            LoadApplication(new App());

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.Delegate = this;

                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
                    Console.WriteLine(granted);
                });
            }
            else
            {
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, new NSSet());
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            var token = Messaging.SharedInstance.FcmToken ?? "";
            Console.WriteLine($"FCM token: {token}");
            UIApplication.SharedApplication.RegisterForRemoteNotifications();

            return base.FinishedLaunching(app, options);
        }

        //Receive Local Notifications
        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {

            UIAlertController okayAlertController = UIAlertController.Create(notification.AlertAction, notification.AlertBody, UIAlertControllerStyle.Alert);
            okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }

        [Export("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {
            App.deviceToken = fcmToken;
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            if (deviceToken == null)
            {
                return;
            }
            Messaging.SharedInstance.ApnsToken = deviceToken;
        }

        public void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        {
            System.Diagnostics.Debug.WriteLine($"FCM Token: {fcmToken}");
        }

        [Obsolete]
        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            new UIAlertView("Error registering push notifications", error.LocalizedDescription, null, "OK", null).Show();
        }

        [Obsolete]
        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            System.Console.WriteLine(userInfo.ToString());
            new UIAlertView("remote  push notifications", userInfo.Description, null, "OK", null).Show();
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            System.Console.WriteLine(nameof(DidReceiveRemoteNotification), userInfo);

            completionHandler(UIBackgroundFetchResult.NewData);
        }

        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            var userInfo = notification.Request.Content.UserInfo;

            Console.WriteLine(userInfo);

            completionHandler(UNNotificationPresentationOptions.Alert);
        }

        // Handle notification messages after display notification is tapped by the user.
        [Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
        public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            var userInfo = response.Notification.Request.Content.UserInfo;

            // Print full message.
            Console.WriteLine(userInfo);

            if (userInfo["Type"] != null)
            {
                //App.notificationType = userInfo["Type"].ToString();
                App.notificationType = "Game Today";
            }

            MessagingCenter.Send<String>("", utils.Constants.msDash);

            completionHandler();
        }
    }
}
