using System;
using System.Collections.Generic;
using System.Text;

namespace VestiMK.Models
{
    public class Station
    {
        public string name { get; set; }
    }

    public class Datum
    {
        public Station station { get; set; }
    }

    public class Co
    {
        public double v { get; set; }
    }

    public class Dew
    {
        public double v { get; set; }
    }

    public class H
    {
        public double v { get; set; }
    }

    public class No2
    {
        public double v { get; set; }
    }

    public class P
    {
        public double v { get; set; }
    }

    public class Pm10
    {
        public double v { get; set; }
    }

    public class So2
    {
        public double v { get; set; }
    }

    public class T
    {
        public double v { get; set; }
    }

    public class W
    {
        public double v { get; set; }
    }

    public class Wg
    {
        public double v { get; set; }
    }
    public class City
    {
        public IList<double> geo { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public double distance { get; set; }
    }
    public class Iaqi
    {
        public Co co { get; set; }
        public Dew dew { get; set; }
        public H h { get; set; }
        public No2 no2 { get; set; }
        public P p { get; set; }
        public Pm10 pm10 { get; set; }
        public So2 so2 { get; set; }
        public T t { get; set; }
        public W w { get; set; }
        public Wg wg { get; set; }
    }

    public class Data
    {
        public string aqi { get; set; }
        public int idx { get; set; }
        public string dominentpol { get; set; }
        public Iaqi iaqi { get; set; }
        public City city { get; set; }
    }


    public class StationAQI
    {
        public string status { get; set; }
        public IList<Datum> data { get; set; }
    }

    public class Example
    {
        public string status { get; set; }
        public Data data { get; set; }
    }

}
