namespace sitecorepro3.Foundation.Indexing.Repositories
{
    using sitecorepro3.Foundation.DependencyInjection;
    using sitecorepro3.Foundation.Indexing.Models;
    using sitecorepro3.Foundation.Indexing.Services;

    [Service(typeof(ISearchServiceRepository))]
    public class SearchServiceRepository : ISearchServiceRepository
    {
        public virtual SearchService Get(ISearchSettings settings)
        {
            return new SearchService(settings);
        }
    }
}