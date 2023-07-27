using Sitecore.Diagnostics;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Pipelines.GetContentEditorWarnings;
using Sitecore.Links;
using Sitecore.SecurityModel;
using System.IO;
using Sitecore.StringExtensions;

namespace sitecorepro3.Feature.MainContent.Processors
{
    public class RobotProcessor : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));
            if (args.Url.QueryString.Contains("robot"))
            {
                string content = GetRobotsTxtContent();

                if (!string.IsNullOrEmpty(content))
                {
                    HttpContext.Current.Response.ContentType = "text/plain";
                    HttpContext.Current.Response.Write(content);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                }
            }



        }
        private string GetRobotsTxtContent()
        {
            string content = string.Empty;
            var itemId = new ID("{D11C52BC-DB7F-4535-B58A-DBB9D668603A}");

            var db = Factory.GetDatabase("master");
                var item = db.GetItem(itemId);

                if (item != null)
                {
                    // Lấy nội dung của trường "Text" của Item
                    using (new SecurityDisabler())
                    {
                        var text = item.Fields["Text"].Value;
                        content = text ?? string.Empty;
                    }
                }
                else
                {
                    Log.Warn($"Robots.txt item not found: {itemId}", this);
                }

                return content;
        }
    }
}