﻿#region using



#endregion

namespace sitecorepro3.Foundation.Indexing.Infrastructure.Fields
{
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Data.Items;
    using sitecorepro3.Foundation.SitecoreExtensions.Extensions;

    public class HasPresentationComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }

        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            Item i = indexable as SitecoreIndexableItem;
            if (i.HasLayout())
            {
                return true;
            }
            return null;
        }
    }
}