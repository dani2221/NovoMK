using System;
using System.Collections.Generic;
using System.Text;

namespace VestiMK.Models
{
    public class NewsContent
    {
        public string title;
        public string category;
        public string site;
        public string desc;
        public string url;
        public List<int> time;
        public string content;
        public string imageURL;
        public string newsSource;
        public string shortcontent;

        public int imageSizeX;
        public int imageSizeY;
    }
}
