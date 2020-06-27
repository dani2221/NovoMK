using System;
using System.Collections.Generic;
using System.Text;

namespace VestiMK.Models
{
    public class CryptoModel
    {
        public string id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public double current_price { get; set; }
        public double price_change_24h { get; set; }
        public double price_change_percentage_24h { get; set; }
    } 
}

