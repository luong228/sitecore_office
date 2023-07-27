using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sitecorepro3.Feature.Header.Controllers
{
    public class HeaderController : Controller
    {
        // GET: Header
        public ActionResult Header()
        {
            return View();
        }
        public ActionResult HeaderHome()
        {
            return View();
        }
    }
}