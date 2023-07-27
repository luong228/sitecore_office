namespace sitecorepro3.Foundation.Indexing.Repositories
{
    using sitecorepro3.Foundation.Indexing.Models;
    using sitecorepro3.Foundation.Indexing.Services;

    public interface ISearchServiceRepository
    {
        SearchService Get(ISearchSettings searchSettings);
    }
}