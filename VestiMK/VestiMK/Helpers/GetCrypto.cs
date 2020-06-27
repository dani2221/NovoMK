using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VestiMK.Models;

namespace VestiMK.Helpers
{
    class GetCrypto
    {
        public async static Task<List<CryptoModel>> GetResult()
        {
            List<CryptoModel> coins = new List<CryptoModel>();
            var url = "https://api.coingecko.com/api/v3/coins/markets?vs_currency=eur&order=market_cap_desc&per_page=5&page=1&sparkline=false";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                var st = JsonConvert.DeserializeObject<List<CryptoModel>>(json);

                coins = st as List<CryptoModel>;
            }

            return coins;
        }
    }
}
