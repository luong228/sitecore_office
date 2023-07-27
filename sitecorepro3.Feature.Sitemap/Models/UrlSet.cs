using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Web.Mvc;
namespace sitecorepro3.Feature.Sitemap.Models
{
    //[XmlRoot("urlset")]
    public class UrlSet
    {
        public UrlSet() { Url = new List<Url>(); }

        ///

        /// Urls collection
        /// 

        //[XmlElement("url")]
        public List<Url> Url { get; set; }

    }
    
}