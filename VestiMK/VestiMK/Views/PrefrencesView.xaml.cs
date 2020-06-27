using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VestiMK.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrefrencesView : ContentPage
    {
        News previousNews;
        int k = 0;
        public PrefrencesView(News previousNews)
        {
            InitializeComponent();
            this.previousNews = previousNews;
            
        }
        public PrefrencesView()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (Preferences.Get("mk", true))
                mk.IsChecked = true;
            else
                mk.IsChecked = false;
            if (Preferences.Get("ek", true))
                ek.IsChecked = true;
            else
                ek.IsChecked = false;
            if (Preferences.Get("sv", true))
                sv.IsChecked = true;
            else
                sv.IsChecked = false;
            if (Preferences.Get("sp", true))
                sp.IsChecked = true;
            else
                sp.IsChecked = false;
            if (Preferences.Get("kl", true))
                kl.IsChecked = true;
            else
                kl.IsChecked = false;

            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            var st = true;
            if (status != PermissionStatus.Granted)
                st = false;
            if (status == PermissionStatus.Granted)
            {
                switchAQI.IsToggled = Preferences.Get("togl", true);
                Preferences.Set("togl", switchAQI.IsToggled);
            } 
            else
            {
                switchAQI.IsToggled = false;
                Preferences.Set("togl", false);
            }
            k++;
        }

        private void mk_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (k != 0)
            {
                if (Preferences.Get("mk", true))
                    Preferences.Set("mk", false);
                else
                    Preferences.Set("mk", true);
            }
        }

        private void sv_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (k != 0)
            {
                if (Preferences.Get("sv", true))
                    Preferences.Set("sv", false);
                else
                    Preferences.Set("sv", true);
            }
        }

        private void ek_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (k != 0)
            {
                if (Preferences.Get("ek", true))
                    Preferences.Set("ek", false);
                else
                    Preferences.Set("ek", true);
            }
        }

        private void sp_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (k != 0)
            {
                if (Preferences.Get("sp", true))
                    Preferences.Set("sp", false);
                else
                    Preferences.Set("sp", true);
            }
        }

        private void kl_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (k != 0)
            {
                if (Preferences.Get("kl", true))
                    Preferences.Set("kl", false);
                else
                    Preferences.Set("kl", true);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if(previousNews!=null)
                previousNews.setRefreshTrue();
        }

        private async void switchAQI_Toggled(object sender, ToggledEventArgs e)
        {
            
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (switchAQI.IsToggled == false && status != PermissionStatus.Granted)
                return;
                if (status == PermissionStatus.Granted)
            {
                if (switchAQI.IsToggled)
                    Preferences.Set("togl", true);
                else
                    Preferences.Set("togl", false);
            }
            else
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status == PermissionStatus.Granted)
                {
                    switchAQI.IsToggled = true;
                    Preferences.Set("togl", true);
                }
                else
                {
                    switchAQI.IsToggled = false;
                    Preferences.Set("togl", false);
                }
            }
        }
    }
}