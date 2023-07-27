using Sitecore.Data;
using Sitecore.Web.UI.WebControls;
using sitecorepro3.Foundation.Indexing.Infrastructure;
using sitecorepro3.Foundation.Indexing.Models;
using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Fields;

namespace sitecorepro3.Feature.News.Indexing
{
    public class NewsIndexingProvider : ProviderBase, ISearchResultFormatter, IQueryPredicateProvider
    {
        public Expression<Func<SearchResultItem, bool>> GetQueryPredicate(IQuery query)
        {
            var fieldNames = new[] { Templates.NewsArticle.Fields.Title_FieldName, Templates.NewsArticle.Fields.Summary_FieldName, Templates.NewsArticle.Fields.Body_FieldName };
            return GetFreeTextPredicateService.GetFreeTextPredicate(fieldNames, query);
        }

        public string ContentType =>  "News";

        public IEnumerable<ID> SupportedTemplates => new[] { Templates.NewsArticle.ID };

        public void FormatResult(SearchResultItem item, ISearchResult formattedResult)
        {
            var contentItem = item.GetItem();
            if (contentItem == null)
            {
                return;
            }

            formattedResult.Title = FieldRenderer.Render(contentItem, Templates.NewsArticle.Fields.Title.ToString());
            formattedResult.Description = FieldRenderer.Render(contentItem, Templates.NewsArticle.Fields.Summary.ToString());
            formattedResult.Media = ((ImageField)contentItem.Fields[Templates.NewsArticle.Fields.Image])?.MediaItem;
            formattedResult.ViewName = "~/Views/News/NewsSearchResult.cshtml";
        }
    }
}