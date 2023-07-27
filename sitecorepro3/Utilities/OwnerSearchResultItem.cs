using System.Runtime.Serialization;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace sitecorepro3.Project.sitecorepro3.Utilities
{
    public class OwnerSearchResultItem : SearchResultItem
    {
        [IndexField("_name")]
        [DataMember]
        public string OwnerName { get; set; }
    }
}