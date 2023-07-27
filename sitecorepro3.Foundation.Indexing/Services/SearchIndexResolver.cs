namespace sitecorepro3.Foundation.Indexing.Services
{
    using Sitecore.ContentSearch;
    using sitecorepro3.Foundation.DependencyInjection;

    [Service]
    public class SearchIndexResolver
    {
        public virtual ISearchIndex GetIndex(SitecoreIndexableItem contextItem)
        {
            return ContentSearchManager.GetIndex(contextItem);
        }
    }
}