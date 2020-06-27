using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VestiMK.Models;

namespace VestiMK.Helpers
{
    public static class AQIAPI
    {
        public async static Task<List<Datum>> GetAvailableSTations()
        {
            List<Datum> stations=new List<Datum>();
            var url = "https://api.waqi.info/map/bounds/?token={token}&latlng=40.842726955720,20.4631520,42.3202595078,22.952377150220";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                var st = JsonConvert.DeserializeObject<StationAQI>(json);

                stations = st.data as List<Datum>;
            }

            return stations;
        }
        public async static Task<Example> GetStationInfo(Datum st)
        {
            var splitted = st.station.name.Split(new[] { "(" }, StringSplitOptions.None)[0];
            var url = "https://api.waqi.info/feed/"+ splitted +"/?token={token}";
            Example ex;
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                Example stt = JsonConvert.DeserializeObject<Example>(json);
                ex = stt;
            }
            return ex;
        }

        public async static Task<Akcii> GetAkcii()
        {
            var url = "http://feeds.mse.mk/service/FreeMSEFeeds.svc/ticker/JSON/{token}";
            Akcii ak = new Akcii();
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                var st = JsonConvert.DeserializeObject<Akcii>(json);

                ak = st as Akcii;
            }

            return ak;
        }
    }
}
