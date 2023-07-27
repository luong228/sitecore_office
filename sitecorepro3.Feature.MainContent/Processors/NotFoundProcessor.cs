using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Pipelines.GetContentEditorWarnings;
using Sitecore.Links;

namespace sitecorepro3.Feature.MainContent.Processors
{
    public class NotFoundProcessor : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));
            if (args.Url.QueryString.Contains("robot")) return;
                if (!Context.PageMode.IsNormal) return;
            if (Context.Database != null && Context.Database.Name == "core")
            {
                return;
            } 

            var sitecoreContext = Sitecore.Context.Item;
            if (sitecoreContext == null || sitecoreContext.Visualization.Layout == null)

            {

                Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase("web");


                var itemNotFound = master.Items.GetItem(new ID("{A996B99E-9DD1-4C3C-AF0A-3C14D40CF558}"));

     
                var itemUrl = LinkManager.GetItemUrl(itemNotFound);
                var listPath = itemUrl.Split('/');
                var finalPath = listPath[0] + "//" + listPath[2] + "/en/office/notfound";
                HttpContext.Current.Response.RedirectPermanent(finalPath);
            }

        }
    }
}