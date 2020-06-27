using MarcTron.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VestiMK.Models
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebNewsView : ContentPage
    {
        int p;
        public WebNewsView(string url,string name,int p)
        {
            InitializeComponent();
            pp.Title = name;
            wb.Source = url;
            this.p = p;

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(p%4==1)
                CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-6638560950207737/4239069970");
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //if(p%4==1)
                ////CrossMTAdmob.Current.ShowInterstitial();
        }
    }
}