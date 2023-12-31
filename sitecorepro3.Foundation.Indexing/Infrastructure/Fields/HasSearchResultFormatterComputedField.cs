﻿#region using



#endregion

namespace sitecorepro3.Foundation.Indexing.Infrastructure.Fields
{
    using System.Linq;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using sitecorepro3.Foundation.Indexing.Repositories;
    using sitecorepro3.Foundation.SitecoreExtensions.Extensions;

    public class HasSearchResultFormatterComputedField : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexItem = indexable as SitecoreIndexableItem;
            if (indexItem == null)
            {
                return null;
            }
            var item = indexItem.Item;

            return IndexingProviderRepository.SearchResultFormatters.Any(p => p.SupportedTemplates.Any(id => item.DescendsFrom(id)));
        }
    }
}