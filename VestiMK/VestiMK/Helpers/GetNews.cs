using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Text;
using System.Xml;
using VestiMK.Models;
using System.Threading.Tasks;

namespace VestiMK.Helpers
{
    class GetNews
    {
        string url;
        bool isMultiple;
        public GetNews(string url,bool isMultiple)
        {
            this.url = url;
            this.isMultiple = isMultiple;
        }

        public async Task<List<NewsContent>> news()
        {
            List<NewsContent> feedItemsList = new List<NewsContent>();

            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                WebResponse webResponse = webRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream();
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(stream);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
                nsmgr.AddNamespace("dc", xmlDocument.DocumentElement.GetNamespaceOfPrefix("dc"));
                nsmgr.AddNamespace("content", xmlDocument.DocumentElement.GetNamespaceOfPrefix("content"));
                XmlNodeList itemNodes = xmlDocument.SelectNodes("rss/channel/item");
                for (int i = 0; i < itemNodes.Count; i++)
                {
                    try
                    {
                        NewsContent feedItem = new NewsContent();

                        if (itemNodes[i].SelectSingleNode("title") != null)
                        {
                            feedItem.title = itemNodes[i].SelectSingleNode("title").InnerText;
                        }
                        if (itemNodes[i].SelectSingleNode("link") != null)
                        {
                            feedItem.url = itemNodes[i].SelectSingleNode("link").InnerText;
                        }
                        if (itemNodes[i].SelectSingleNode("pubDate") != null)
                        {
                            var time = itemNodes[i].SelectSingleNode("pubDate").InnerText;
                            feedItem.time = getHour(time);
                        }
                        if (itemNodes[i].SelectSingleNode("description") != null)
                        {
                            feedItem.desc = itemNodes[i].SelectSingleNode("description").InnerText;

                        }
                        if (itemNodes[i].SelectSingleNode("content:encoded", nsmgr) != null)
                        {
                            feedItem.content = itemNodes[i].SelectSingleNode("content:encoded", nsmgr).InnerText;
                        }
                        else
                        {
                            feedItem.content = feedItem.desc;
                        }
                        feedItem.imageURL = getImage(feedItem.content);


                        var sourcename = url.Split(new[] { "//" }, StringSplitOptions.None)[1];
                        feedItem.newsSource = sourcename.Split(new[] { "/" }, StringSplitOptions.None)[0];

                        feedItemsList.Add(feedItem);
                        if (i > 1 && !isMultiple)
                            break;
                    }
                    catch(Exception e)
                    {
                        
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }

            return feedItemsList;
        }
        public async Task<NewsContent> news(int i)
        {
            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                WebResponse webResponse = webRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream();
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(stream);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
                nsmgr.AddNamespace("dc", xmlDocument.DocumentElement.GetNamespaceOfPrefix("dc"));
                nsmgr.AddNamespace("content", xmlDocument.DocumentElement.GetNamespaceOfPrefix("content"));
                XmlNodeList itemNodes = xmlDocument.SelectNodes("rss/channel/item");
                NewsContent feedItem = new NewsContent();

                    if (itemNodes[i].SelectSingleNode("title") != null)
                    {
                        feedItem.title = itemNodes[i].SelectSingleNode("title").InnerText;
                    }
                    if (itemNodes[i].SelectSingleNode("link") != null)
                    {
                        feedItem.url = itemNodes[i].SelectSingleNode("link").InnerText;
                    }
                    if (itemNodes[i].SelectSingleNode("pubDate") != null)
                    {
                        var time = itemNodes[i].SelectSingleNode("pubDate").InnerText;
                        feedItem.time = getHour(time);
                    }
                    if (itemNodes[i].SelectSingleNode("description") != null)
                    {
                        feedItem.desc = itemNodes[i].SelectSingleNode("description").InnerText;

                    }
                    if (itemNodes[i].SelectSingleNode("content:encoded", nsmgr) != null)
                    {
                        feedItem.content = itemNodes[i].SelectSingleNode("content:encoded", nsmgr).InnerText;
                    }
                    else
                    {
                        feedItem.content = feedItem.desc;
                    }
                    feedItem.imageURL = getImage(feedItem.content);


                    var sourcename = url.Split(new[] { "//" }, StringSplitOptions.None)[1];
                    feedItem.newsSource = sourcename.Split(new[] { "/" }, StringSplitOptions.None)[0];
                   
                    
                return feedItem;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        string getImage(string full)
        {
            try
            {
                var code = full.Split(new[] { "src=\"" }, StringSplitOptions.None)[1];
                var fin = code.Split(new[] { "\"" }, StringSplitOptions.None)[0];
                return fin;
            }
            catch(Exception e)
            {
                try
                {
                    var code = full.Split(new[] { "src=\'" }, StringSplitOptions.None)[1];
                    var fin = code.Split(new[] { "\'" }, StringSplitOptions.None)[0];
                    return fin;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        List<int> getHour(string full)
        {
            try
            {
                List<int> smh = new List<int>();
                var ph = full.Split(new[] { "2020 " }, StringSplitOptions.None)[1];
                var hour = ph.Split(new[] { ":" }, StringSplitOptions.None);
                smh.Add(Int32.Parse(hour[0]));
                smh.Add(Int32.Parse(hour[1]));
                var second = hour[2].Split(new[] { " " }, StringSplitOptions.None)[0];
                smh.Add(Int32.Parse(second));
                return smh;
            }catch(Exception)
            {
                return null;
            }
        }
    }
}    
