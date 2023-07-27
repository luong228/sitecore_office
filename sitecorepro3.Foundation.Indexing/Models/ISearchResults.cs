namespace sitecorepro3.Foundation.Indexing.Models
{
    using System.Collections.Generic;

    public interface ISearchResults
    {
        IEnumerable<ISearchResultFacet> Facets { get; }
        IEnumerable<ISearchResult> Results { get; }
        int TotalNumberOfResults { get; }
    }
}