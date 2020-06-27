using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VestiMK.Helpers;
using VestiMK.Models;
using Xamarin.Forms;

namespace VestiMK
{
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();

        }
        protected async override void OnAppearing()
        {
           
            base.OnAppearing();
        }
    }
}
