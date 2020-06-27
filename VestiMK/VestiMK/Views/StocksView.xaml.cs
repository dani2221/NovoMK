using MarcTron.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VestiMK.Helpers;
using VestiMK.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VestiMK.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StocksView : ContentPage
    {
        private int k = 0;
        Dictionary<string, string> words = new Dictionary<string, string>();
        List<GetTickerJSONResult> ordered;
        List<GetTickerJSONResult> all;
        List<CryptoModel> cryresult;
        public StocksView()
        {
            InitializeComponent();
        }
        protected  override void OnAppearing()
        {
            base.OnAppearing();
            
            if (!CrossMTAdmob.Current.IsInterstitialLoaded())
                CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-6638560950207737/4239069970");
            if (k == 0)
            {
                showAkcii();
                fillDict();
                searchBar.TextChanged += OnTextChanged;
                ICommand refreshCommand = new Command(async () =>
                {
                    k = 0;
                    showAkcii();
                    k++;
                    refresh.IsRefreshing = false;
                });
                refresh.Command = refreshCommand;
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
                showAkcii();
                return;
            }
            List<GetTickerJSONResult> searchList = new List<GetTickerJSONResult>();
            foreach (var st in ordered)
            {
                string source = st.Description.ToLower();
                foreach (KeyValuePair<string, string> pair in words)
                {
                    source = source.Replace(pair.Key, pair.Value);
                }
                if (source.Contains(searchTerm.ToLower()))
                    searchList.Add(st);
            }
           ordered = searchList;
            showAkcii();
        }

        private async void showAkcii()
        {
            stek.Children.Clear();
            if(k==0)
            {
                Akcii akciiList=null;
                await Task.Run(async () =>
                {
                    akciiList = await AQIAPI.GetAkcii();
                });
                await Task.Run(async () =>
                {
                    cryresult = await GetCrypto.GetResult();
                });
                Label lbb = new Label() { Text = "Сеуште не е отворена берзата\nПробајте повторно подоцна", HorizontalTextAlignment = TextAlignment.Center };
                if(akciiList.GetTickerJSONResult.Count==0)
                {
                    stek.Children.Add(lbb);

                    Frame frfr1 = new Frame() { CornerRadius = 5, HasShadow = true, Margin = new Thickness(10, 5), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, BackgroundColor = Color.FromHex("#121212") };
                    Grid grgr1 = new Grid();
                    Label kripto = new Label() { Text = "Криптовалути:", FontSize = 20, FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#6cf58e"), HorizontalTextAlignment = TextAlignment.Start };
                    grgr1.Children.Add(kripto, 0, 0);
                    Grid.SetColumnSpan(kripto, 2);

                    for (int i = 0; i < cryresult.Count; i++)
                    {
                        Label ime = new Label() { Text = cryresult[i].name, FontSize = 14, FontAttributes = FontAttributes.Bold, TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Start };
                        Label perCh = new Label() { Text = cryresult[i].price_change_percentage_24h.ToString() + "%", FontSize = 14, TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Center };
                        Label cena = new Label() { Text = cryresult[i].current_price.ToString() + "€", FontSize = 14, TextColor = Color.White, HorizontalTextAlignment = TextAlignment.End };
                        if (cryresult[i].price_change_percentage_24h >= 0)
                            perCh.TextColor = Color.FromHex("#6cf58e");
                        else
                            perCh.TextColor = Color.FromHex("#FF7595");

                        grgr1.Children.Add(ime, 0, i + 1);
                        grgr1.Children.Add(perCh, 1, i + 1);
                        grgr1.Children.Add(cena, 2, i + 1);
                    }
                    frfr1.Content = grgr1;
                    stek.Children.Add(frfr1);
                    return;
                }

                
                ordered = akciiList.GetTickerJSONResult.OrderBy(f => f.AvgPerChange).ToList();
                all = ordered;
            }
            ordered.Reverse();
            int forBrojac = 0;
            foreach (var akcija in ordered)
            {
                if(forBrojac==2)
                {
                    Frame frfr1 = new Frame() { CornerRadius = 5, HasShadow = true, Margin = new Thickness(10, 5), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, BackgroundColor = Color.FromHex("#121212") };
                    Grid grgr1 = new Grid();
                    Label kripto = new Label() { Text = "Криптовалути:", FontSize = 20, FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#6cf58e"), HorizontalTextAlignment = TextAlignment.Start };
                    grgr1.Children.Add(kripto, 0, 0);
                    Grid.SetColumnSpan(kripto, 2);

                    for (int i=0;i<cryresult.Count;i++)
                    {
                        Label ime = new Label() { Text = cryresult[i].name, FontSize = 14, FontAttributes = FontAttributes.Bold, TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Start };
                        Label perCh = new Label() { Text = cryresult[i].price_change_percentage_24h.ToString()+"%", FontSize = 14, TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Center };
                        Label cena = new Label() { Text = cryresult[i].current_price.ToString()+ "€", FontSize = 14, TextColor = Color.White,HorizontalTextAlignment=TextAlignment.End};
                        if(cryresult[i].price_change_percentage_24h>=0)
                            perCh.TextColor = Color.FromHex("#6cf58e");
                        else
                            perCh.TextColor = Color.FromHex("#FF7595");

                        grgr1.Children.Add(ime, 0, i+1);
                        grgr1.Children.Add(perCh, 1, i+1);
                        grgr1.Children.Add(cena, 2, i+1);
                    }
                    frfr1.Content = grgr1;
                    stek.Children.Add(frfr1);

                }
                Frame frfr = new Frame() { CornerRadius = 5, HasShadow = true,Margin=new Thickness(10,5), HorizontalOptions=LayoutOptions.Start, VerticalOptions=LayoutOptions.Start, BackgroundColor=Color.FromHex("#121212")};
                Grid grgr = new Grid();

                grgr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2, GridUnitType.Star) });
                grgr.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                grgr.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(5, GridUnitType.Star) });
                grgr.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3, GridUnitType.Star) });

                string titleText = String.Format("{0} ({1})", akcija.Description, akcija.Symbol);
                Label title = new Label() { Text = titleText, FontSize = 15, FontAttributes = FontAttributes.Bold, TextColor = Color.White };
                Label priceNow = new Label() { Text = akcija.AvgPrice.ToString()+" den.", FontSize = 15, FontAttributes = FontAttributes.Bold , TextColor=Color.White};


                string perChange = akcija.AvgPerChange.ToString() +"%";
                if (akcija.AvgPerChange > 0)
                    perChange = "+" + akcija.AvgPerChange.ToString()+"%";
                string absChange = akcija.AvgAbsChange.ToString()+" den.";
                if (akcija.AvgAbsChange > 0)
                    absChange = "+" + akcija.AvgAbsChange.ToString()+ " den.";

                if (akcija.AvgPerChange > 0)
                    priceNow.TextColor = Color.FromHex("#6cf58e");
                else
                    priceNow.TextColor = Color.FromHex("#FF7595");

                Label percent = new Label() { Text = perChange, FontSize = 13, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, TextColor = Color.White };
                Label abs = new Label() { Text = absChange, FontSize = 13, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, TextColor=Color.White };

                grgr.Children.Add(title, 0, 0);
                grgr.Children.Add(priceNow, 0, 1);
                grgr.Children.Add(percent, 1, 0);
                grgr.Children.Add(abs, 1, 1);

                frfr.Content = grgr;
                stek.Children.Add(frfr);
                forBrojac++;

            }
            if(ordered.Count<2)
            {
                Frame frfr1 = new Frame() { CornerRadius = 5, HasShadow = true, Margin = new Thickness(10, 5), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Start, BackgroundColor = Color.FromHex("#121212") };
                Grid grgr1 = new Grid();
                Label kripto = new Label() { Text = "Криптовалути:", FontSize = 20, FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#6cf58e"), HorizontalTextAlignment = TextAlignment.Start };
                grgr1.Children.Add(kripto, 0, 0);
                Grid.SetColumnSpan(kripto, 2);

                for (int i = 0; i < cryresult.Count; i++)
                {
                    Label ime = new Label() { Text = cryresult[i].name, FontSize = 14, FontAttributes = FontAttributes.Bold, TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Start };
                    Label perCh = new Label() { Text = cryresult[i].price_change_percentage_24h.ToString() + "%", FontSize = 14, TextColor = Color.White, HorizontalTextAlignment = TextAlignment.Center };
                    Label cena = new Label() { Text = cryresult[i].current_price.ToString() + "€", FontSize = 14, TextColor = Color.White, HorizontalTextAlignment = TextAlignment.End };
                    if (cryresult[i].price_change_percentage_24h >= 0)
                        perCh.TextColor = Color.FromHex("#6cf58e");
                    else
                        perCh.TextColor = Color.FromHex("#FF7595");

                    grgr1.Children.Add(ime, 0, i + 1);
                    grgr1.Children.Add(perCh, 1, i + 1);
                    grgr1.Children.Add(cena, 2, i + 1);
                }
                frfr1.Content = grgr1;
                stek.Children.Add(frfr1);
            }
        }
        private void fillDict()
        {
            words.Add("а", "a");
            words.Add("б", "b");
            words.Add("в", "v");
            words.Add("г", "g");
            words.Add("д", "d");
            words.Add("е", "e");
            words.Add("ј", "j");
            words.Add("з", "z");
            words.Add("и", "i");
            words.Add("к", "k");
            words.Add("л", "l");
            words.Add("м", "m");
            words.Add("н", "n");
            words.Add("о", "o");
            words.Add("п", "p");
            words.Add("р", "r");
            words.Add("с", "s");
            words.Add("т", "t");
            words.Add("у", "u");
            words.Add("ф", "f");
            words.Add("х", "h");
            words.Add("ц", "c");
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

            //CrossMTAdmob.Current.ShowInterstitial();
            k = 0;
            showAkcii();
            k++;
        }
        private string getDenesenURL()
        {
            DateTime dt = new DateTime();
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;
            day--;
            var monthString = month.ToString();
            var dayString = day.ToString();
            if (month < 10)
                monthString = "0" + month.ToString();
            if (day < 10)
                dayString = "0" + day.ToString();
            
           
            string url = String.Format("https://www.mse.mk/Repository/Reports/{0}/{1}.{2}.{3}kr.xls", year, dayString, monthString, year);
            return url;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(getDenesenURL()));
        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PrefrencesView());
        }
    }
}