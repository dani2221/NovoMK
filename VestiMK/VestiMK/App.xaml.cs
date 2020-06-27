using System;
using VestiMK.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VestiMK
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPageGradientHeader(new MainPage())
            {
                LeftColor = Color.FromHex("#109F8D"),
                RightColor = Color.FromHex("#36ED81")
            };

            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#121212");
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.FromHex("#FF026");
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
