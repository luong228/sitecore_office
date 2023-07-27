using sitecorepro3.Feature.News.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using sitecorepro3.Foundation.Indexing.Repositories;
using sitecorepro3.Foundation.SitecoreExtensions.Extensions;

namespace sitecorepro3.Feature.News.Controllers
{
    public class News1Controller : Controller
    {
       
        public News1Controller()
        {
            
        }
        public ActionResult NewsList()
        {
           
            return this.View("NewsList");
        }
        public ActionResult NewsDetail()
        {
            //var item = this.Repository.Get(RenderingContext.Current.Rendering.Item);
            return this.View("NewsDetail");
        }
        public ActionResult LatestNews()
        {
            return this.View("LatestNews");
        }
    }
}