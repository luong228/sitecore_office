using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.RenderField;

namespace sitecorepro3.Project.sitecorepro3.Utilities
{
    public class RenderExternalLinks
    {
        public void Process(RenderFieldArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.ArgumentNotNull(args.FieldTypeKey, "args.FieldTypeKey");

            if (args.FieldTypeKey != "rich text"
                || String.IsNullOrEmpty(args.FieldValue)
                || !Context.PageMode.IsNormal)
                return;

            // Load the HTML into the HtmlAgilityPack
            var doc = new HtmlDocument { OptionWriteEmptyNodes = true };
            doc.LoadHtml(args.Result.FirstPart);

            // Search for all links
            var aTag = doc.DocumentNode.SelectNodes("//a");
            if (aTag == null || aTag.Count == 0)
                return;

            foreach (HtmlNode node in aTag)
            {
                // Look for links to YouTube
                if (node.Attributes["href"] != null && node.Attributes["data-transform"] != null && node.ParentNode != null
                    && (node.Attributes["href"].Value.Contains("youtube.com") || node.Attributes["href"].Value.Contains("flickr.com")))
                {
                    node.ParentNode.InnerHtml
                        = node.ParentNode.InnerHtml.Replace(node.OuterHtml,
                            "");
                }
            }
            // Replace the Rich Text content with the modified content
            args.Result.FirstPart = doc.DocumentNode.OuterHtml;
        }
    }
}