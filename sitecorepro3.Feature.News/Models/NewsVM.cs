using Sitecore.Links;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Data.Items;

namespace sitecorepro3.Feature.News.Models
{
    public class NewsVM
    {
        public Item Item { get; set; }

        public MvcHtmlString Title
        {
            get { return GetFieldValue("Title"); }
        }
        public MvcHtmlString Summary
        {
            get { return GetFieldValue("Summary"); }
        }
        public MvcHtmlString Body
        {
            get { return GetFieldValue("Body"); }
        }
        public MvcHtmlString Date
        {
            get { return GetFieldValue("Date"); }
        }
        public string Url
        {
            get { return Item.Paths.Path; }
        }
        // used link manager

        public string ImageUrl
        {
            get
            {
                Sitecore.Data.Fields.ImageField imgField = ((Sitecore.Data.Fields.ImageField)Item.Fields["Image"]);
                return Sitecore.Resources.Media.MediaManager.GetMediaUrl(imgField.MediaItem);

            }
        }
        public string GetUrl
        {
            get
            {
                var language = Sitecore.Context.Language;
                UrlOptions options = new UrlOptions();
                options.AlwaysIncludeServerUrl = false; // Set to false to generate a relative URL
                options.Language = language;
                options.EmbedLanguage(language);
                options.LanguageEmbedding = LanguageEmbedding.Always;
                return Sitecore.Links.LinkManager.GetItemUrl(Item, options);

            }
            //var itemlink = GetFieldValue("Quantity");
        }
        public MvcHtmlString GetFieldValue(string fieldName)
        {
            return new MvcHtmlString(FieldRenderer.Render(Item, fieldName));
        }
    }
}