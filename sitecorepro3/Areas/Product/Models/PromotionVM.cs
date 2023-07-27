 using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;

namespace sitecorepro3.Project.sitecorepro3.Areas.Product.Models
{
    public class PromotionVM
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
        public MvcHtmlString GetFieldValue(string fieldName)
        {
            return new MvcHtmlString(FieldRenderer.Render(Item, fieldName));
        }
    }
}