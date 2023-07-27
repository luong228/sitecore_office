using System.Runtime.Serialization;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace sitecorepro3.Project.sitecorepro3.Utilities
{
    public class ProductSearchResultItem : SearchResultItem
    {
        [IndexField("name_t")]
        [DataMember]
        public string ProductName { get; set; }
        [IndexField("description_t")]
        [DataMember]
        public string Description { get; set; }
        [IndexField("price_tf")]
        [DataMember]
        public float Price { get; set; }
        [IndexField("quantity_tf")]
        [DataMember]
        public float Quantity { get; set; }
        [IndexField("vat_tf")]
        [DataMember]
        public float Vat { get; set; }
        [IndexField("owner_t")]
        public string OwnerName { get; set; }

    }
}