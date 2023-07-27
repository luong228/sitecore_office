using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Web.UI.WebControls;

namespace sitecorepro3.Project.sitecorepro3.Areas.Product.Models
{
    public class ProductVM
    {
        public Item Item { get; set; }

        public MvcHtmlString Name
        {
            get { return GetFieldValue("Name"); }
        }
        public MvcHtmlString Description
        {
            get { return GetFieldValue("Description"); }
        }
        public MvcHtmlString Price
        {
            get { return GetFieldValue("Price"); }
        }
        public MvcHtmlString Quantity
        {
            get { return GetFieldValue("Quantity"); }
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
                UrlOptions options = new UrlOptions();
                options.AlwaysIncludeServerUrl = false; // Set to false to generate a relative URL
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