using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using sitecorepro3.Feature.BasicContent.Models;

namespace sitecorepro3.Feature.BasicContent.Controllers
{
    public class BasicController : Controller
    {
        // GET: Basic
        public ActionResult LoginBox()
        {
            return View();
        }
        public ActionResult ContentHome()
        {
            return View();
        }
        public ActionResult BlockFunction()
        {
            return View();
        }
        public ActionResult Popup()
        {
            return View();
        }

        public ActionResult Weather()
        {
            Database database = Sitecore.Data.Database.GetDatabase("master");
            var itemId = new ID("{EA056730-AA3C-4C2A-81DB-746AE35E6FDC}");
            Item item = database.GetItem(itemId);
            var weather = new Weather()
            {
                WeatherInfo = item.Fields["Text"].Value
            };
            return View(weather);
        }
        }
    }