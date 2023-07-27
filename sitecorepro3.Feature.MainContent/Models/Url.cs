using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace sitecorepro3.Feature.MainContent.Models
{
    public class Url
    {
        ///

        /// Location Parameter
        /// 

        [XmlElement("loc")]
        public string Loc { get; set; }

        ///

        /// Last modified on
        /// 

        [XmlElement("lastmod")]
        public string Lastmod { get; set; }

        //Add required properties here like changefreq, priority
    }
}