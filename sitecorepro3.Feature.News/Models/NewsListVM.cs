using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sitecorepro3.Feature.News.Models
{
    public class NewsListVM
    {
        public IList<NewsVM> NewsList { get; set; }
    }
}