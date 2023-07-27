using sitecorepro3.Feature.News.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using sitecorepro3.Feature.News.Models;
using sitecorepro3.Foundation.Indexing.Repositories;
using sitecorepro3.Foundation.SitecoreExtensions.Extensions;

namespace sitecorepro3.Feature.News.Controllers
{
    public class NewsController : Controller
    {
      
        public NewsController( )
        {
    
        }

        public ActionResult NewsListApi()
        {
            //ViewData["NewsResult"] = _newsApiController.GetAllNews();
            return View();
        }
        public ActionResult NewsList()
        {
            //var items = this.Repository.Get(RenderingContext.Current.Rendering.Item);
            var pageSize = RenderingContext.Current.Rendering.GetIntegerParameter("pagesize", 5);
            string page = Request.QueryString["page"];
            var pageNumber = 1;

            if (!string.IsNullOrEmpty(page)) pageNumber = Convert.ToInt32(page);

                var database = Sitecore.Context.Database;
            string query = $"/{Sitecore.Context.Item.Paths.FullPath}/*[@@templatename='NewsTemplate']";
            var searchResults = database.SelectItems(query);


            var viewModel = new NewsListVM()
            {
                NewsList = searchResults.Select(i =>
                    new NewsVM
                    {
                        Item = i,
                    }).Skip((pageNumber-1)*pageSize).Take(pageSize).ToList()
            };
            return this.View("NewsList", viewModel);
        }
        public ActionResult NewsDetail()
        {
            //var item = this.Repository.Get(RenderingContext.Current.Rendering.Item);

            Item contextItem = Sitecore.Context.Item;

            var news = new NewsVM()
            {
                Item = contextItem
            };
            return View(news);
            //return this.View("NewsDetail");
        }
        public ActionResult LatestNews()
        {
            //TODO: change to parameter template
            var count = RenderingContext.Current.Rendering.GetIntegerParameter("count", 1);

            var database = Sitecore.Context.Database;
            string query = $"/{Sitecore.Context.Item.Paths.FullPath}/News/*[@@templatename='NewsTemplate']";
            var searchResults = database.SelectItems(query);


            var viewModel = new NewsListVM()
            {
                NewsList = searchResults.Select(i =>
                    new NewsVM
                    {
                        Item = i,
                    }).Take(count).ToList()
            };
            return this.View("LatestNews", viewModel);
        }

    }
}