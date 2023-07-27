using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using Sitecore;
using Sitecore.Data.Items;
using sitecorepro3.Feature.Sitemap.Models;
using sitecorepro3.Foundation.SitecoreExtensions.Extensions;

namespace sitecorepro3.Feature.Sitemap.Controllers
{
    public class SitemapController : Controller
    {
        // GET: Sitemap
        public ActionResult Sitemap()
        {
            try
            {
                // Homepage of the Website.
                // Start path will give homepage including Multisite.
                var homeId = new ID("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}");
                Item homepage = Context.Database.GetItem(homeId);
                var ser = new XmlSerializer(typeof(Models.UrlSet));

                var urlSet = new Models.UrlSet();

                //Create node of Homepage in Sitemap.
                var tmpurlset = new List<Url>();
                var config = AppendLanguage();

                if (!ExcludeItemFromSitemap(homepage))
                {
                    tmpurlset.Add(new Url
                    {
                        Loc = GetAbsoluteLink(LinkManager.GetItemUrl(homepage, new UrlOptions() { LanguageEmbedding = (config == 2 ? LanguageEmbedding.Always : (config == 1 ? LanguageEmbedding.AsNeeded : LanguageEmbedding.Never)) })),
                        Lastmod = homepage.Statistics.Updated.ToString("yyyy-MM-dd hh:mm:ss")
                    });
                }

                // Get all decendants of Homepage to create full Sitemap.
                var childrens = homepage.Axes.GetDescendants();
                //Remove the items whose templateid is in exclude list
                var finalcollection = childrens.Where(x => !ExcludeItemFromSitemap(x)).Where(x => x.HasLayout()).ToList();
                //ONly get item has presentation

                tmpurlset.AddRange(finalcollection.Select(childItem => new Url
                {
                    Loc = GetAbsoluteLink(LinkManager.GetItemUrl(childItem, new UrlOptions() { LanguageEmbedding = (config == 2 ? LanguageEmbedding.Always : (config == 1 ? LanguageEmbedding.AsNeeded : LanguageEmbedding.Never)) })),
                    Lastmod = childItem.Statistics.Updated.ToString("yyyy-MM-dd hh:mm:ss")
                }));

                // Populate created collection to right object
                urlSet.Url = tmpurlset;
                var urlSet1 = new UrlSet()
                {
                    Url = tmpurlset
                };
                //Response Ends Here
                return View(urlSet1);
            }
            catch (Exception ex)
            {
                Log.Error("Error - Sitemap.xml.", ex, this);
            }
            return View();
        }
        private static string GetAbsoluteLink(string relativeUrl)
        {
            //return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + relativeUrl;
            return relativeUrl;
        }

        ///

        /// Append language or not in URL to return language specific sitemap.xml
        /// 

        ///
        private static int AppendLanguage()
        {
            return string.IsNullOrEmpty(Sitecore.Configuration.Settings.GetSetting("LanguageEmbedForSitemap")) ? 0 : System.Convert.ToInt32((Sitecore.Configuration.Settings.GetSetting("LanguageEmbedForSitemap")));
        }

        ///

        /// This method will get a list of excluding template ids and will check if the passed item is in
        /// 

        ///
        ///
        private static bool ExcludeItemFromSitemap(Item objItem)
        {
            //Check if the item is having any version
            if (objItem.Versions.Count > 0)
            {
                var excludeItems = Sitecore.Configuration.Settings.GetSetting("ExcludeSitecoreItemsByTemplatesInSitemap");
                var collection = excludeItems.Split(',').ToList();
                return collection.Contains(objItem.TemplateID.ToString());
            }
            return true;
        }
    }
}