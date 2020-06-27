using System;
using System.Collections.Generic;
using System.Text;

namespace VestiMK.Models
{
    public class GetTickerJSONResult
    {
        public double AvgAbsChange { get; set; }
        public double AvgPerChange { get; set; }
        public double AvgPrice { get; set; }
        public string Description { get; set; }
        public string Symbol { get; set; }
        public string TierCode { get; set; }
    }

    public class Akcii
    {
        public IList<GetTickerJSONResult> GetTickerJSONResult { get; set; }
    }


}
