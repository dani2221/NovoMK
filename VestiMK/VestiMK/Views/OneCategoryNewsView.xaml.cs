using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VestiMK.Helpers;
using VestiMK.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VestiMK.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OneCategoryNewsView : ContentPage
    {
        string category;
        private bool dobreak = false;
        int p = 0;
        public OneCategoryNewsView(string category)
        {
            this.category = category;
            
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(category.Equals(Sites.mak))
                titl.Title = "Македонија";
            if(category.Equals(Sites.svet))
                titl.Title = "Свет";
            if(category.Equals(Sites.ekonomija))
                titl.Title = "Економија";
            if(category.Equals(Sites.sport))
                titl.Title = "Спорт";
            if(category.Equals(Sites.kultura))
                titl.Title = "Култура";
            if (p == 0)
            {
                getNews();
            }
        }

        private async void getNews()
        {
             List<List<NewsContent>> allnews = new List<List<NewsContent>>();
            ActivityIndicator ai = new ActivityIndicator() { IsRunning = true, BackgroundColor = Color.Black, Color = Color.FromHex("#FF0266") };
            mainStek.Children.Insert(0,ai);
            if (!category.Equals(Sites.ekonomija))
            {
                await Task.Run(() =>
                {
                    Parallel.For(0, Sites.sites.Count(), i =>
                    {
                        var newsList = Sites.oneNewsOrdered(category, Sites.sites[i]);
                        allnews.Add(newsList.Result);
                    });

                });
            }
            else
            {
                await Task.Run(() =>
                {
                    Parallel.For(0, Sites.economySites.Count(), i =>
                    {
                        var newsList = Sites.oneNewsOrdered(category, Sites.economySites[i]);
                        allnews.Add(newsList.Result);
                    });

                });
            }
            for(int i=0;i<allnews.Count;i++)
            {
                while (allnews[i].Contains(null))
                    allnews[i].Remove(null);
            }
            List<NewsContent> all = new List<NewsContent>();
            for (int i = 0; i < min(allnews); i++)
            {
                for (int j = 0; j < allnews.Count(); j++)
                {
                    all.Add(allnews[j][i]);
                }
            }
            for (int i = 0; i < all.Count - 1; i++)
            {
                for (int j = i + 1; j < all.Count/2; j++)
                {
                    string phrase1 = all[i].title.ToLower();
                    string phrase2 = all[j].title.ToLower();
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
                    if (percent > 50)
                    {
                        NewsContent newnw = null;
                        await Task.Run(async () =>
                        {
                            newnw = await Sites.oneNews(all[j].category, all[j].site, 2);
                        });
                        if (!newnw.Equals(null))
                            all[j] = newnw;
                    }

                }
            }
            ai.IsRunning = false;
            mainStek.Children.Remove(ai);
            showNews(all);

        }
        private async void showNews(List<NewsContent> allNews)
        {
            int brojac = 0;
            int stekBrojac = 0;
            
                for(int j=0;j<allNews.Count();j++)
                {
                    var oneNew = allNews[j];
                    if(brojac%5==0)
                    {
                        
                        //za prvo
                        Label src = new Label() { Text = oneNew.newsSource, FontSize = 12, TextColor = Color.LightGray };
                        Image img = new Image() { Source = oneNew.imageURL, Aspect = Aspect.AspectFit, HeightRequest = 150 };
                        Label title = new Label() { Text = oneNew.title, FontAttributes = FontAttributes.Bold, FontSize = 20, TextColor = Color.White };
                        Label time = new Label() { Text = await getTimePosted(oneNew.time), FontSize = 10, TextColor = Color.LightGray };

                        TapGestureRecognizer gs = new TapGestureRecognizer();
                        gs.Tapped += async (s, e) =>
                        {
                            await Navigation.PushAsync(new WebNewsView(oneNew.url, oneNew.newsSource.ToUpper(), p));
                            p++;
                        };
                        StackLayout st = new StackLayout() { Margin = new Thickness(5) };
                        st.GestureRecognizers.Add(gs);
                        st.Children.Add(src);
                        st.Children.Add(img);
                        st.Children.Add(title);
                        st.Children.Add(time);
                        mainGrid.Children.Add(st, 0, stekBrojac);
                        Grid.SetColumnSpan(st, 2);
                        brojac++;
                        stekBrojac += 1;
                    }
                    else
                    {
                        Label src1 = new Label() { Text = oneNew.newsSource, FontSize = 9, TextColor = Color.LightGray };
                        Image img1 = new Image() { Source = oneNew.imageURL, Aspect = Aspect.AspectFit, HeightRequest = 70 };
                        Label title1 = new Label() { Text = oneNew.title, FontAttributes = FontAttributes.Bold, FontSize = 16, TextColor = Color.White };
                        Label time1 = new Label() { Text = await getTimePosted(oneNew.time), FontSize = 9, TextColor = Color.LightGray };

                        TapGestureRecognizer gs1 = new TapGestureRecognizer();
                        gs1.Tapped += async (s, e) =>
                        {
                            StackLayout stetk = (StackLayout)s;
                            Label titl = (Label)stetk.Children[4];
                            Label srcc = (Label)stetk.Children[0];

                            await Navigation.PushAsync(new WebNewsView(titl.Text, srcc.Text.ToUpper(), p));
                            p++;
                        };
                        StackLayout st1 = new StackLayout() { Margin = new Thickness(5) };
                        st1.GestureRecognizers.Add(gs1);
                        st1.Children.Add(src1);
                        st1.Children.Add(img1);
                        st1.Children.Add(title1);
                        st1.Children.Add(time1);
                        st1.Children.Add(new Label() { Text = oneNew.url, IsVisible = false });

                        if (brojac%5 == 1)
                            mainGrid.Children.Add(st1, 0, stekBrojac);
                        if (brojac%5 == 2)
                        {
                            mainGrid.Children.Add(st1, 1, stekBrojac);
                            stekBrojac++;
                        }

                        if (brojac % 5 == 3)
                            mainGrid.Children.Add(st1, 0, stekBrojac);
                        if (brojac % 5 == 4)
                        {
                            mainGrid.Children.Add(st1, 1, stekBrojac);
                            stekBrojac++;
                        }
                        brojac++;
                    }
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
            }
            catch
            {
                return null;
            }
        }
        protected override void OnDisappearing()
        {
            dobreak = true;
            base.OnDisappearing();
        }
        private int min(List<List<NewsContent>> nw)
        {
            int m = nw[0].Count;
            foreach(var pp in nw)
            {
                if (pp.Count < m)
                    m = pp.Count;
            }
            return m;
        }


    }
   
}