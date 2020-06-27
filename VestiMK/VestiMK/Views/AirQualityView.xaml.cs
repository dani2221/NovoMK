using MarcTron.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VestiMK.Helpers;
using VestiMK.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VestiMK.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AirQualityView : ContentPage
    {
        private bool orderLocation;
        List<Example> ordered;
        List<Example> all;
        int k = 0;
        public AirQualityView()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
         
            if (!CrossMTAdmob.Current.IsInterstitialLoaded())
                CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-6638560950207737/4239069970");
            if (k == 0)
            {
                searchBar.TextChanged += OnTextChanged;
                ICommand refreshCommand = new Command(async () =>
                {
                    k = 0;
                    stek.Children.Clear();
                    ////CrossMTAdmob.Current.ShowInterstitial();
                    await showAQI();
                    k++;
                    refresh.IsRefreshing = false;
                });
                refresh.Command = refreshCommand;

                refresh.IsRefreshing = true;
                k++;
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBar src = (SearchBar)sender;
            var searchTerm = src.Text;
            
            if (searchTerm == "")
            {
                ordered = all;
                showAQI();
                return;
            }
            List<Example> searchList = new List<Example>();
            foreach(var st in ordered)
            {
                if (st.data.city.name.ToLower().Contains(searchTerm.ToLower()))
                    searchList.Add(st);
            }
            ordered = searchList;
            showAQI();
        }

        async Task showAQI()
        {
            if (k == 0)
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status == PermissionStatus.Granted)
                {
                    if (Preferences.Get("togl", true))
                        orderLocation = true;
                    else
                        orderLocation = false;

                }
                else
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if (status == PermissionStatus.Granted)
                    {
                        orderLocation = true;
                        Preferences.Set("togl", true);
                    }
                    else
                    {
                        orderLocation = false;
                        Preferences.Set("togl", false);
                    }
                }
                List<Datum> stations=null;
                await Task.Run(async () =>
                {
                    stations = await AQIAPI.GetAvailableSTations();
                });
                if(stations.Count==0)
                {
                    Label lbb = new Label() { Text = "There is a problem with getting data.\nPlease try again later", HorizontalTextAlignment = TextAlignment.Center };
                    stek.Children.Add(lbb);
                    return;
                }

                List<Example> stt = new List<Example>();
                List<Example> problemStations = new List<Example>();
                foreach (var st in stations)
                {
                    Example station = null;
                    await Task.Run(async () =>
                    {
                        station = await AQIAPI.GetStationInfo(st);
                    });
                    
                    try
                    {
                        if(orderLocation)
                        {
                            var location = await Geolocation.GetLastKnownLocationAsync();
                            var distance = Location.CalculateDistance(station.data.city.geo[0], station.data.city.geo[1], location, DistanceUnits.Kilometers);
                            station.data.city.distance = distance;
                        }

                        int notImportant=0;
                        var well = Int32.TryParse(station.data.aqi,out notImportant);
                        if (well)
                            stt.Add(station);
                        else
                        {
                            problemStations.Add(station);
                            station.data.aqi = "0";
                        }
                    }
                    catch(Exception e)
                    {
                        problemStations.Add(station);
                        station.data.aqi = "0";
                    }
                    
                }
                if (!orderLocation)
                    ordered = stt.OrderBy(f => Int32.Parse(f.data.aqi)).ToList();
                else
                {
                    ordered = stt.OrderBy(f => f.data.city.distance).ToList();
                    ordered.Reverse();
                }
                ordered.AddRange(problemStations);
                all = ordered;
            }
            stek.Children.Clear();
            
            for (int i = ordered.Count-1;i>=0;i--)
            {
                if (ordered[i].data.city.name.ToLower().Contains("kosovo"))
                    continue;
                Frame frfr = new Frame() { CornerRadius = 5, HasShadow = true, Margin = new Thickness(10,5),BackgroundColor=Color.FromHex("#121212")};
                Grid grgr = new Grid();


                grgr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(3, GridUnitType.Star) });
                grgr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                grgr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                grgr.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                grgr.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                grgr.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                grgr.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                Label name, AQI;
                
                name = new Label() { FontSize = 18, FontAttributes = FontAttributes.Bold, Text = ordered[i].data.city.name,TextColor=Color.White};
                try
                {
                    AQI = new Label() { FontSize = 23, FontAttributes = FontAttributes.Bold, Text = ordered[i].data.aqi.ToString()+ "\nAQI", HorizontalTextAlignment=TextAlignment.End };
                    if (Int32.Parse(ordered[i].data.aqi) <= 50)
                        AQI.TextColor = Color.FromHex("#6cf58e");
                    else if (Int32.Parse(ordered[i].data.aqi) > 50 && Int32.Parse(ordered[i].data.aqi) < 100)
                        AQI.TextColor = Color.FromHex("#ff954f");
                    else
                        AQI.TextColor = Color.FromHex("#FF7595");
                }
                catch(Exception e)
                {
                    AQI = new Label() { FontSize = 23, FontAttributes = FontAttributes.Bold, Text = "0\nAQI", HorizontalTextAlignment = TextAlignment.End };
                    AQI.TextColor = Color.FromHex("#6cf58e");
                }
                
                

                Label pm10;
                if(ordered[i].data.iaqi.pm10!=null)
                    pm10 = new Label() { FontSize = 15,Text=ordered[i].data.iaqi.pm10.v.ToString(), HorizontalTextAlignment = TextAlignment.Center,TextColor=Color.White };
                else
                    pm10 = new Label() { FontSize = 15, Text ="непознато", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White };

                Label coo;
                if (ordered[i].data.iaqi.co!= null)
                    coo = new Label() { FontSize = 15, Text = ordered[i].data.iaqi.co.v.ToString(), HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                else
                    coo = new Label() { FontSize = 15, Text = "непознато", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White };

                Label so2o;
                if(ordered[i].data.iaqi.so2!=null)
                    so2o = new Label() { FontSize = 15, Text = ordered[i].data.iaqi.so2.v.ToString(), HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                else
                    so2o = new Label() { FontSize = 15, Text = "непознато", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                Label no2o;
                if(ordered[i].data.iaqi.no2!=null)
                    no2o= new Label() { FontSize = 15, Text = ordered[i].data.iaqi.no2.v.ToString(), HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                else
                    no2o = new Label() { FontSize = 15, Text = "непознато", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White };

                Label pm10LABEL = new Label() { FontSize = 8, Text = "PM10", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                Label coLABEL = new Label() { FontSize = 8, Text = "CO", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                Label so2LABEL = new Label() { FontSize = 8, Text = "SO2", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                Label no2LABEL = new Label() { FontSize = 8, Text = "NO2", HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.White };


                grgr.Children.Add(name, 0, 0);
                Grid.SetColumnSpan(name, 3);
                grgr.Children.Add(AQI, 3, 0);
                grgr.Children.Add(pm10, 0, 1);
                grgr.Children.Add(coo, 1, 1);
                grgr.Children.Add(so2o, 2, 1);
                grgr.Children.Add(no2o, 3, 1);
                grgr.Children.Add(pm10LABEL, 0, 2);
                grgr.Children.Add(coLABEL, 1, 2);
                grgr.Children.Add(so2LABEL, 2, 2);
                grgr.Children.Add(no2LABEL, 3, 2);

                frfr.Content = grgr;
                Console.WriteLine(i.ToString());
                stek.Children.Add(frfr);
   
                CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-6638560950207737/4239069970");
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            k = 0;
            stek.Children.Clear();
            ////CrossMTAdmob.Current.ShowInterstitial();
            showAQI();
            k++;
        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PrefrencesView());
        }
    }
}