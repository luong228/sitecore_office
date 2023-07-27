using System.Runtime.Serialization;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace sitecorepro3.Foundation.Indexing.Models
{
    public class NewsSearchResult : SearchResultItem
    {
        [IndexField("_name")]
        [DataMember]
        public string OwnerName { get; set; }
    }
}