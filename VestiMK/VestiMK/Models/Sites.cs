using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VestiMK.Helpers;

namespace VestiMK.Models
{
    public class Sites
    {
        public static List<String> sites = new List<String>();
        public static string KURIR = "https://kurir.mk/category/";                
        public static string A1ON = "https://a1on.mk/category/";                //na angliski + /sport1 + /region1
        public static string PLUSINFO = "https://plusinfo.mk/category/";        // /makedoni-a +/bs-kultura + /bs-sport
        public static string TELMA = "https://telma.com.mk/kategorija/";         
        public static string TV21 = "https://tv21.tv/mk/category/";             //nema kulrura
        public static string CENTAR = "https://centar.mk/blog/category/";    
        public static string OPSERVER = "https://opserver.mk/category/";
        public static string MAKFAX = "https://makfax.com.mk/";
        public static string ALFA = "https://alfa.mk/kategorija/vesti/";
        public static string NOVAMAK = "https://www.novamakedonija.com.mk/category/";
        public static string PECAT = "https://www.slobodenpecat.mk/category/vesti/"; //svet=planeta
        public static string NEZAVISEN = "https://nezavisen.mk/kategorija/";
        public static string LOKALNO = "https://lokalno.mk/category/";
        public static string LIBERTAS = "https://www.libertas.mk/category/";
        public static string KANAL5 = "https://kanal5.com.mk/rss.aspx?id=";
        public static string BRIF = "https://www.brif.mk/category/";

        public static List<string> economySites = new List<String>();
        public static string economyA1ON = "https://a1on.mk/category/economy/feed/";
        public static string economyKURIR = "https://kurir.mk/category/makedonija/ekonomija/feed/";
        public static string economyFRONTLINE = "https://frontline.mk/category/ekonomi-a/feed/";
        public static string economyNEZAVISEN = "https://nezavisen.mk/kategorija/ekonomija/feed/";
        public static string economyLOKALNO = "https://lokalno.mk/category/biznis/feed/";
        public static string economyBIZNIS = "https://www.biznisvesti.mk/category/ekonomija/feed/";
        public static string economyDENAR = "https://denar.mk/category/ekonomija/feed";
        public static string economyEKONIMII = "https://ekonomski.mk/kategorija/ekonomija/feed/";
        public static string economyBRIF = "https://www.brif.mk/category/ekonomija/feed/";

        //categories
        public static List<string> categories = new List<string>();
        public static string mak = "makedonija";
        public static string svet = "svet";
        public static string kultura = "kultura";
        public static string sport = "sport";
        public static string ekonomija = "ekonomija";

       
        public async static Task<NewsContent> oneNews(string category,string site,int pp)
        {
            if(sites.Count==0)
                addElements();
            var url = site + category + "/feed/";
            if (category != ekonomija)
            {
                
                if (site.Equals("https://a1on.mk/category/"))
                {
                    if (category.Equals("makedonija"))
                        url = site + "macedonia" + "/feed/";
                    if (category.Equals("svet"))
                        url = site + "world" + "/feed/";
                    if (category.Equals("kultura"))
                        url = site + "culture" + "/feed/";
                }
                if (site.Equals("https://plusinfo.mk/category/"))
                {
                    if (category.Equals("makedonija"))
                        url = site + "makedoni-a" + "/feed/";
                    if (category.Equals("kultura"))
                        url = site + "bs-kultura" + "/feed/";
                    if (category.Equals("sport"))
                        url = site + "bs-sport" + "/feed/";
                }
                if (site.Equals("https://a1on.mk/category/"))
                {
                    if (category.Equals("sport"))
                        url = site + "sport1" + "/feed/";                 //fixxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx not working +/region1
                }
                if (site.Equals(PECAT))
                {
                    if (category.Equals(svet))
                        url = site + "planeta" + "/feed/";
                }
                if (site.Equals(TV21))
                {
                    if (category.Equals(kultura))
                        url = site + "magazin/kultura-magazin" + "/feed/";
                }
                if (site.Equals(NOVAMAK))
                {
                    if (category.Equals(kultura))
                        url = site + "zivot" + "/feed/";
                }
                if(site.Equals(KANAL5))
                {
                    if (category.Equals(mak))
                        url = site + "2";
                    if (category.Equals(svet))
                        url = site + "3";
                    if (category.Equals(kultura))
                        url = site + "7";
                    if (categories.Equals(sport))
                        url = site + "8";
                }
                if (site.Equals(BRIF))
                {
                    if (category.Equals(kultura))
                        url = site + "magazin" + "/feed/";
                }
            }
            else
            {
                url = site;
            }
            GetNews gn = new GetNews(url,false);
            Random rn = new Random();
            try
            {
                var s = await gn.news(rn.Next(0,2)+pp);
                if (s == null)
                    throw new Exception();
                s.category = category;
                s.site = site;
                return s;
            }catch(Exception e)
            {
                if (!site.Equals(A1ON))
                {
                    if (category != ekonomija)

                        return await oneNews(category, A1ON, 2);
                    else
                        return await oneNews(category, economyA1ON, 2);
                }
                else
                {
                    return null;
                }
            }
        }

        public async static Task<List<NewsContent>> oneNewsOrdered(string category, string site)
        {
            if (sites.Count == 0)
                addElements();
            var url = site + category + "/feed/";
            if (category != ekonomija)
            {
                if (site.Equals("https://a1on.mk/category/"))
                {
                    if (category.Equals("makedonija"))
                        url = site + "macedonia" + "/feed/";
                    if (category.Equals("svet"))
                        url = site + "world" + "/feed/";
                    if (category.Equals("kultura"))
                        url = site + "culture" + "/feed/";
                }
                if (site.Equals("https://plusinfo.mk/category/"))
                {
                    if (category.Equals("makedonija"))
                        url = site + "makedoni-a" + "/feed/";
                    if (category.Equals("kultura"))
                        url = site + "bs-kultura" + "/feed/";
                    if (category.Equals("sport"))
                        url = site + "bs-sport" + "/feed/";
                }
                if (site.Equals("https://a1on.mk/category/"))
                {
                    if (category.Equals("sport"))
                        url = site + "sport1" + "/feed/";                 //fixxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx not working +/region1
                }
                if (site.Equals(PECAT))
                {
                    if (category.Equals(svet))
                        url = site + "planeta" + "/feed/";
                }
                if (site.Equals(TV21))
                {
                    if (category.Equals(kultura))
                        url = site + "magazin/kultura-magazin" + "/feed/";
                }
                if (site.Equals(NOVAMAK))
                {
                    if (category.Equals(kultura))
                        url = site + "zivot" + "/feed/";
                }
                if (site.Equals(KANAL5))
                {
                    if (category.Equals(mak))
                        url = site + "2";
                    if (category.Equals(svet))
                        url = site + "3";
                    if (category.Equals(kultura))
                        url = site + "7";
                    if (categories.Equals(sport))
                        url = site + "8";
                }
                if(site.Equals(BRIF))
                {
                    if(category.Equals(kultura))
                        url = site + "magazin"+"/feed/";
                }
            }
            else
            {
                url = site;
            }
            GetNews gn = new GetNews(url, false);
            try
            {
                var s = await gn.news();
                try
                {
                    foreach (var pipi in s)
                    {
                        pipi.category = category;
                        pipi.site = site;
                    }
                }catch(Exception e)
                {
                    return null;
                }
                return s;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        

       public static void addElements()
       {
            sites.Add(KURIR);
            sites.Add(A1ON);
            sites.Add(PLUSINFO);
            //sites.Add(TELMA);
                //sites.Add(TV21);
                //sites.Add(CENTAR);
            sites.Add(OPSERVER);
            sites.Add(MAKFAX);
            //sites.Add(ALFA);
            sites.Add(NOVAMAK);
            sites.Add(PECAT);
                // sites.Add(NEZAVISEN);
            sites.Add(LOKALNO);
            sites.Add(LIBERTAS);
            sites.Add(KANAL5);
            sites.Add(BRIF);

                //economySites.Add(economyNEZAVISEN);
            economySites.Add(economyLOKALNO);
            economySites.Add(economyKURIR);
                //economySites.Add(economyFRONTLINE);
            economySites.Add(economyA1ON);
            //economySites.Add(economyBIZNIS);
            economySites.Add(economyDENAR);
            economySites.Add(economyEKONIMII);
            economySites.Add(economyBRIF);

            categories.Add(mak);
            categories.Add(svet);
            categories.Add(ekonomija);
            categories.Add(sport);
            categories.Add(kultura);
       }
        
        
    }
    public static class Listt
    {
        private static Random rng = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

}
