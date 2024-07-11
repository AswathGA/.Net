using Draftsy.api;
using Draftsy.helpers;
using Draftsy.models;
using Draftsy.pages.Business.events;
using Draftsy.pages.dashboard;
using Draftsy.pages.detail;
using Draftsy.pages.detail.chatDetail;
using Draftsy.pages.Gallery;
using Draftsy.pages.league.draft;
using Draftsy.pages.league.draftDash;
using Draftsy.pages.list;
using Draftsy.pages.login;
using Draftsy.pages.onboarding;
using Draftsy.utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using static Draftsy.models.LoginVo;

namespace Draftsy
{
    public partial class App : Application
    {
        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }

        public static UserVo userVo { get; set; }
        public static YearsVo yearsVo { get; set; }

        public static YearsVo.YearsData selectedYear { get; set; } = new YearsVo.YearsData(yearID: 1, year: "2022");
        public static SportsVo.Data selectedSport { get; set; } = new SportsVo.Data(sportId: 1, sportType: "Baseball", sportName: "College Baseball", imageBlue: "https://mypyfn.blob.core.windows.net/sport-icons/baseball.png", imageGray: "https://mypyfn.blob.core.windows.net/sport-icons/baseball_gray.png");

        public static List<string> yearNames = new List<string>();

        public static PlayerVo playerVo { get; set; }

        public static string deviceToken { get; set; } = "";

        public static string notificationType { get; set; } = "";
        public static string notificationLeagueId { get; set; } = "";
        public static string notificationLeagueDetail { get; set; } = "";

        public static bool isAfterCreateLeague = false;
        public static bool isFromCreateLeagueToHome = false;

        public static bool isFromBackground = true;

        public static Page currentPage;
        public App()
        {
            InitializeComponent();
            Constants.loader = new CommonLoader();

            if (Application.Current.Properties.ContainsKey(Constants.userVo))
            {
                userVo = JsonConvert.DeserializeObject<UserVo>(Application.Current.Properties[Constants.userVo].ToString());

                if (notificationType == "Game Today" || notificationType.ToLower() == "game")
                {
                    MainPage = new NavigationPage(new DashPage())
                    {
                        BarBackgroundColor = Color.FromHex("#222222"),
                        BarTextColor = Color.White,
                    };
                }
                else if(notificationType == "draft")
                {
                    goToDraft();
                }
                else if(notificationType == "chat")
                {
                    ChatRoomsVo.Chats chats = new ChatRoomsVo.Chats();
                    chats.id = 1;
                    chats.type = "personal";
                    chats.name = "Notification";

                    MainPage = new NavigationPage(new ChatDetailPage(chats))
                    {
                        BarBackgroundColor = Color.FromHex("#222222"),
                        BarTextColor = Color.White,
                    };
                }
                else if (notificationType.ToLower() == "calender")
                {
                    MainPage = new NavigationPage(new MycalenderPage())
                    {
                        BarBackgroundColor = Color.FromHex("#222222"),
                        BarTextColor = Color.White,
                    };
                }
                else
                {
                    if (App.userVo.isOnboarding)
                    {
                        MainPage = new NavigationPage(new OnboardingPage())
                        {
                            BarBackgroundColor = Color.FromHex("#222222"),
                            BarTextColor = Color.White,
                        };
                    } else
                    {
                        MainPage = new NavigationPage(new DashPage())
                        {
                            BarBackgroundColor = Color.FromHex("#222222"),
                            BarTextColor = Color.White,
                        };

                    }
                }

                notificationType = "";
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage())
                {
                    BarBackgroundColor = Color.FromHex("#F1F1F1"),
                    BarTextColor = Color.Black,
                };
            }


            MessagingCenter.Subscribe<String>(this, Constants.msDash, (obj) =>
            {
                MainPage = new NavigationPage(new DashPage())
                {
                    BarBackgroundColor = Color.FromHex("#222222"),
                    BarTextColor = Color.White,
                };
            }); 
            
            MessagingCenter.Subscribe<String>(this, Constants.msLeagueCreate, (obj) =>
            {
                isAfterCreateLeague = true;
                MainPage = new NavigationPage(new DashPage(isForLeague: true))
                {
                    BarBackgroundColor = Color.FromHex("#222222"),
                    BarTextColor = Color.White,
                };
            });

            MessagingCenter.Subscribe<String>(this, Constants.msLogin, (obj) =>
            {
                MainPage = new NavigationPage(new LoginPage())
                {
                    BarBackgroundColor = Color.FromHex("#F1F1F1"),
                    BarTextColor = Color.Black,
                };
            });
            
            MessagingCenter.Subscribe<String>(this, Constants.msOnBoarding, (obj) =>
            {
                MainPage = new NavigationPage(new OnboardingPage())
                {
                    BarBackgroundColor = Color.FromHex("#222222"),
                    BarTextColor = Color.White,
                };
            });
        }

        private async void goToDraft()
        {
            var leagueDetail = new LeagueDetailVo();
            leagueDetail.Data = new Data();
            leagueDetail.Data.LeagueDetails = JsonConvert.DeserializeObject<LeagueDetails>(App.notificationLeagueDetail);

            Constants.leagueDetail = leagueDetail;
            MainPage = new NavigationPage(new DraftHomePage())
            {
                BarBackgroundColor = Color.FromHex("#222222"),
                BarTextColor = Color.White,
            };
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
