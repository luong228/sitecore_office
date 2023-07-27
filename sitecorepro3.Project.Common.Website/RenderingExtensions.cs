using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Data;
using Sitecore.Mvc.Presentation;
using sitecorepro3.Foundation.SitecoreExtensions.Extensions;

namespace sitecorepro3.Project.Common.Website
{
    public static class RenderingExtensions
    {
        public static string GetContainerClass(this Rendering rendering)
        {
            return rendering.IsContainerFluid() ? "container-fluid" : "container";
        }
        public static bool IsContainerFluid(this Rendering rendering)
        {
            return MainUtil.GetBool(rendering.Parameters[Constants.HasContainerLayoutParameters.IsFluid], false);
        }
        public static string GetBackgroundClass(this Rendering rendering)
        {
            var id = MainUtil.GetID(rendering.Parameters[Constants.BackgroundLayoutParameters.Background] ?? "", null);
            if (ID.IsNullOrEmpty(id))
                return "";
            var item = rendering.RenderingItem.Database.GetItem(id);
            return item?[Templates.Style.Fields.Class] ?? "";
        }
    }
}