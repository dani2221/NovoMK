using MarcTron.Plugin;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    public partial class News : ContentPage
    {
        int k = 0;
        int p = 0;

        public News()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            //var nju = Sites.generateListNews();
            //Preferences.Clear();
            if (!CrossMTAdmob.Current.IsInterstitialLoaded())
                CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-6638560950207737/4239069970");
            if (k == 0)
            {

                ICommand refreshCommand = new Command(async () =>
                {
                    //if (CrossMTAdmob.Current.IsInterstitialLoaded() && p != 0)
                        //CrossMTAdmob.Current.ShowInterstitial();
                    await generateNEWS();
                    
                    refresh.IsRefreshing = false;
                });
                refresh.Command = refreshCommand;

                refresh.IsRefreshing = true;
            }
            k++;
            base.OnAppearing();
        }
        public async Task generateNEWS()
        { 
            frameM.IsVisible = true;
            //adM.IsVisible = true;
            frameS.IsVisible = true;
            //adS.IsVisible = true;
            frameK.IsVisible = true;
            //adK.IsVisible = true;
            frameSp.IsVisible = true;
            //adSp.IsVisible = true;
            frameE.IsVisible = true;
            //adE.IsVisible = true;
            if (Sites.sites.Count==0)
                Sites.addElements();
                MAKEDONIJA.Children.Clear();
                SVET.Children.Clear();
                EKONOMIJA.Children.Clear();
                KULTURA.Children.Clear();
                SPORT.Children.Clear();

            foreach (var cat in Sites.categories)
            {
                if (cat.Equals(Sites.mak) && Preferences.Get("mk", true) == false)
                {
                    frameM.IsVisible = false;
                    //adM.IsVisible = false;
                    continue;
                }
                if (cat.Equals(Sites.svet) && Preferences.Get("sv", true) == false)
                {
                    frameS.IsVisible = false;
                    //adS.IsVisible = false;
                    continue;
                }
                if (cat.Equals(Sites.kultura) && Preferences.Get("kl", true) == false)
                {
                    frameK.IsVisible = false;
                    //adK.IsVisible = false;
                    continue;
                }
                if (cat.Equals(Sites.sport) && Preferences.Get("sp", true) == false)
                {
                    frameSp.IsVisible = false;
                    //adSp.IsVisible = false;
                    continue;
                }
                if (cat.Equals(Sites.ekonomija) && Preferences.Get("ek", true) == false)
                {
                    frameE.IsVisible = false;
                    //adE.IsVisible = false;
                    continue;
                }
                List<string> sites;

                if (!cat.Equals(Sites.ekonomija))
                {
                    Sites.sites.Shuffle();
                    sites = Sites.sites;
                }
                else
                {
                    Sites.economySites.Shuffle();
                    sites = Sites.economySites;
                }
                //int k = (int)Math.Floor(Preferences.Get("slajder", 0.3) * 6)+1;
                //BlockingCollection<NewsContent> results = new BlockingCollection<NewsContent>();
                List<NewsContent> results = new List<NewsContent>();
                await Task.Run(() =>
                {
                    Parallel.For(0, 5, i => 
                    {
                        try
                        {
                            var tt = Sites.oneNews(cat, sites[i], 0);
                            results.Add(tt.Result);
                        }catch(Exception e)
                        {

                        }
                    });

                });
                while(results.Contains(null))
                    results.Remove(null);
                for(int i=0;i<results.Count-1;i++)
                {
                    for(int j=i+1;j<results.Count;j++)
                    {
                        string phrase1 = results[i].title.ToLower();
                        string phrase2 = results[j].title.ToLower();
                        string[] words2 = phrase2.Split(' ');
                        double isti = 0.0;
                         if (phrase1.Split(' ')[0].Equals(words2[0]))
                            isti += 15;
                        foreach (var wrd2 in words2)
                        {
                            if (phrase1.Contains(wrd2))
                                isti += 1;
                        }
                        var percent = (isti / words2.Length) * 100;
                        if(percent>50)
                        {
                            NewsContent newnw = null;
                            await Task.Run(async()=>
                            {
                                newnw = await Sites.oneNews(results[j].category, results[j].site, 2);
                            });
                            if(!newnw.Equals(null))
                                results[j] = newnw;
                        }

                    }
                }

                await shownewNews(results);


            }
            
            CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-6638560950207737/4239069970");
        }
        async Task shownewNews(List<NewsContent> mi)
        {
            //za prvo
            Label src = new Label() { Text = mi[0].newsSource, FontSize = 12, TextColor = Color.LightGray };
            Image img = new Image() { Source = mi[0].imageURL, Aspect = Aspect.AspectFit,HeightRequest=150};
            Label title = new Label() { Text = mi[0].title, FontAttributes = FontAttributes.Bold, FontSize = 20, TextColor=Color.White};
            Label time = new Label() { Text = await getTimePosted(mi[0].time), FontSize = 10, TextColor=Color.LightGray};

            TapGestureRecognizer gs = new TapGestureRecognizer();
            gs.Tapped += async (s, e) =>
            {
                await Navigation.PushAsync(new WebNewsView(mi[0].url, mi[0].newsSource.ToUpper(), p));
                p++;
            };
            StackLayout st = new StackLayout() { Margin = new Thickness(5)};
            st.GestureRecognizers.Add(gs);
            st.Children.Add(src);
            st.Children.Add(img);
            st.Children.Add(title);
            st.Children.Add(time);

            if (mi[0].category.Equals(Sites.mak))
                MAKEDONIJA.Children.Add(st);
            if (mi[0].category.Equals(Sites.svet))
                SVET.Children.Add(st);
            if (mi[0].category.Equals(Sites.kultura))
                KULTURA.Children.Add(st);
            if (mi[0].category.Equals(Sites.sport))
                SPORT.Children.Add(st);
            if (mi[0].category.Equals(Sites.ekonomija))
                EKONOMIJA.Children.Add(st);

            //za drugite
            Grid gr = new Grid();
            gr.RowDefinitions.Add(new RowDefinition());
            gr.RowDefinitions.Add(new RowDefinition());
            gr.ColumnDefinitions.Add(new ColumnDefinition());
            gr.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i=1;i<mi.Count;i++)
            {
                Label src1 = new Label() { Text = mi[i].newsSource, FontSize = 9, TextColor = Color.LightGray };
                Image img1 = new Image() { Source = mi[i].imageURL, Aspect = Aspect.AspectFit, HeightRequest=70 };
                Label title1 = new Label() { Text = mi[i].title, FontAttributes = FontAttributes.Bold, FontSize = 16, TextColor = Color.White };
                Label time1 = new Label() { Text = await getTimePosted(mi[i].time), FontSize = 9, TextColor = Color.LightGray };

                TapGestureRecognizer gs1 = new TapGestureRecognizer();
                gs1.Tapped += async (s, e) =>
                {
                    StackLayout stetk= (StackLayout)s;
                    Label titl = (Label)stetk.Children[4];
                    Label srcc = (Label)stetk.Children[0];

                    await Navigation.PushAsync(new WebNewsView(titl.Text, srcc.Text.ToUpper(), p));
                    p++;
                };
                StackLayout st1 = new StackLayout() { Margin = new Thickness(5)};
                st1.GestureRecognizers.Add(gs1);
                st1.Children.Add(src1);
                st1.Children.Add(img1);
                st1.Children.Add(title1);
                st1.Children.Add(time1);
                st1.Children.Add(new Label() { Text = mi[i].url, IsVisible = false });

                if (i == 1)
                    gr.Children.Add(st1, 0, 0);
                if (i == 2)
                    gr.Children.Add(st1, 0, 1);
                if (i == 3)
                    gr.Children.Add(st1, 1, 0);
                if (i == 4)
                    gr.Children.Add(st1, 1, 1);

            }

            if (mi[0].category.Equals(Sites.mak))
                MAKEDONIJA.Children.Add(gr);
            if (mi[0].category.Equals(Sites.svet))
                SVET.Children.Add(gr);
            if (mi[0].category.Equals(Sites.kultura))
                KULTURA.Children.Add(gr);
            if (mi[0].category.Equals(Sites.sport))
                SPORT.Children.Add(gr);
            if (mi[0].category.Equals(Sites.ekonomija))
                EKONOMIJA.Children.Add(gr);

            BoxView devider = new BoxView() { HeightRequest = 1, BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
            Label more = new Label() { Text = "Повеќе вести за оваа категорија", FontSize = 13, TextColor = Color.FromHex("#FF0266"), HorizontalOptions= LayoutOptions.End};
            StackLayout stt = new StackLayout() { Margin=new Thickness(5)};
            
            stt.Children.Add(devider);
            stt.Children.Add(more);
            stt.Children.Add(new Label() { Text = mi[0].category, IsVisible = false });

            TapGestureRecognizer gs2 = new TapGestureRecognizer();
            gs2.Tapped += async (s, e) =>
            {
                StackLayout se = (StackLayout)s;
                var cat = (Label)se.Children[2];

                await Navigation.PushAsync(new OneCategoryNewsView(cat.Text));
            };
            stt.GestureRecognizers.Add(gs2);

            if (mi[0].category.Equals(Sites.mak))
                MAKEDONIJA.Children.Add(stt);
            if (mi[0].category.Equals(Sites.svet))
                SVET.Children.Add(stt);
            if (mi[0].category.Equals(Sites.kultura))
                KULTURA.Children.Add(stt);
            if (mi[0].category.Equals(Sites.sport))
                SPORT.Children.Add(stt);
            if (mi[0].category.Equals(Sites.ekonomija))
                EKONOMIJA.Children.Add(stt);
            

        }

        async Task showNews(NewsContent mi)
        {
            var debugg = mi.shortcontent;
            Frame frfr = new Frame() { CornerRadius = 5, HasShadow = true, HeightRequest = 350 };

            TapGestureRecognizer gs = new TapGestureRecognizer();
            gs.Tapped += async (s, e) =>
            {
                await Navigation.PushAsync(new WebNewsView(mi.url, mi.newsSource.ToUpper(), p));
                p++;
            };
            Grid grgr = new Grid() { VerticalOptions = LayoutOptions.Center };
            frfr.Content = grgr;
            if (mi.category.Equals(Sites.mak))
                MAKEDONIJA.Children.Add(frfr);
            if (mi.category.Equals(Sites.svet))
                SVET.Children.Add(frfr);
            if (mi.category.Equals(Sites.kultura))
                KULTURA.Children.Add(frfr);
            if (mi.category.Equals(Sites.sport))
                SPORT.Children.Add(frfr);
            if (mi.category.Equals(Sites.ekonomija))
                EKONOMIJA.Children.Add(frfr);
            grgr.GestureRecognizers.Add(gs);
            grgr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(4, GridUnitType.Star) });
            var pipi = new RowDefinition()
            {
                Height = new GridLength(7, GridUnitType.Star)
            };
            grgr.RowDefinitions.Add(pipi);
            //grgr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2.5, GridUnitType.Star) });
            grgr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
            double scalex = mainDisplayInfo.Width / mi.imageSizeX;

            Label title = new Label() { Text = mi.title, FontSize = 20, FontAttributes = FontAttributes.Bold };
            Image img = null;
            Label content = null;
            if (mi.imageURL != null && !mi.site.Equals(Sites.TELMA) && !mi.imageURL.Contains("youtube"))
                img = new Image() { Source = mi.imageURL, Aspect = Aspect.AspectFit, HeightRequest = 700 };
            else
                content = new Label() { Text = mi.shortcontent, FontSize = 16, FontAttributes = FontAttributes.Italic };
            Label source = new Label() { Text = mi.newsSource, FontSize = 10, HorizontalTextAlignment = TextAlignment.Start, VerticalTextAlignment = TextAlignment.End };
            Label time = new Label() { Text = await getTimePosted(mi.time), FontSize = 10, HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.End };
            bool nothing = false;
            grgr.Children.Add(title, 0, 0);
            Grid.SetColumnSpan(title, 2);

            if (mi.imageURL != null && !mi.site.Equals(Sites.TELMA))
            {
                grgr.Children.Add(img, 0, 1);
                Grid.SetColumnSpan(img, 2);
            }
            else
            {
                if (content != null)
                {
                    grgr.Children.Add(content, 0, 1);
                    Grid.SetColumnSpan(content, 2);
                }
                else
                {
                    grgr.RowDefinitions.Remove(pipi);
                    frfr.HeightRequest = 100;
                    nothing = true;
                }

            }
            if (!nothing)
            {
                grgr.Children.Add(source, 0, 2);
                grgr.Children.Add(time, 1, 2);
            }
            else
            {
                grgr.Children.Add(source, 0, 1);
                grgr.Children.Add(time, 1, 1);
            }
        
        }

        async Task<string> getTimePosted(List<int> times)
        {
            try
            {
                DateTime dt = DateTime.Now.ToLocalTime();
                var h = dt.Hour - times[0] - 2;
                var m = dt.Minute - times[1];

                if (dt.Minute - times[1] < 0)
                    m = 60 - Math.Abs(dt.Minute - times[1]);
                string fin = "";
                if (h == 0)
                    fin = "пред " + m.ToString() + " минути";
                if (h == 1)
                    fin = "пред 1 час и " + m.ToString() + " минути";
                if (h > 1)
                    fin = "пред " + h.ToString() + " часа и " + m.ToString() + " минути";
                if (h == 0 && m <= 3)
                    fin = "тукушто објавено";
                return fin;
            }catch(Exception e)
            {
                return "";
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if(CrossMTAdmob.Current.IsInterstitialLoaded())
                //CrossMTAdmob.Current.ShowInterstitial();
            await generateNEWS();
        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PrefrencesView(this));
        }
        public void setRefreshTrue()
        {
            refresh.IsRefreshing = true;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OneCategoryNewsView(Sites.mak));
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OneCategoryNewsView(Sites.svet));
        }

        private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OneCategoryNewsView(Sites.ekonomija));
        }

        private async void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OneCategoryNewsView(Sites.sport));
        }

        private async void TapGestureRecognizer_Tapped_4(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OneCategoryNewsView(Sites.kultura));
        }
    }
}