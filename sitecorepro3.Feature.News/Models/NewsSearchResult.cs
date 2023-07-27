using System.Runtime.Serialization;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Web.UI.XslControls;

namespace sitecorepro3.Feature.News.Models
{
    public class NewsSearchResult : SearchResultItem
    {
        [IndexField("title_t")]
        [DataMember]
        public string Title { get; set; }
        [IndexField("summary_t")]
        [DataMember]
        public string Summary { get; set; }

        [IndexField("body_t")]
        [DataMember]
        public string Body { get; set; }
        [IndexField("imageurl_s")]
        [DataMember]
        public string ImageUrl { get; set; }
        [IndexField("date_tdt")]
        [DataMember]
        public Date Date { get; set; }
    }
}