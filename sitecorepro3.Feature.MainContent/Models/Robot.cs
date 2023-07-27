using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;

namespace sitecorepro3.Feature.MainContent.Models
{
    public class Robot
    {
        public Item Item { get; set; }

        public MvcHtmlString Text
        {
            get { return GetFieldValue("Text"); }
        }
        public MvcHtmlString GetFieldValue(string fieldName)
        {
            return new MvcHtmlString(FieldRenderer.Render(Item, fieldName));
        }
    }
}