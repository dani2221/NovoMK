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
            var url = "https://api.waqi.info/map/bounds/?token=28c01e031df543ae6d049aa42f4c9be8a2703fd4&latlng=40.842726955720,20.4631520,42.3202595078,22.952377150220";

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
            var url = "https://api.waqi.info/feed/"+ splitted +"/?token=28c01e031df543ae6d049aa42f4c9be8a2703fd4";
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
            var url = "http://feeds.mse.mk/service/FreeMSEFeeds.svc/ticker/JSON/1cda207b-174c-498c-9e16-23530c6f6da6";
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
