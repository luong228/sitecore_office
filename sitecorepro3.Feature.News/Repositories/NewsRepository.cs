using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Pipelines.GetContentEditorWarnings;
using sitecorepro3.Feature.News.Models;
using sitecorepro3.Foundation.Indexing.Models;
using sitecorepro3.Foundation.Indexing.Repositories;

namespace sitecorepro3.Feature.News.Repositories
{
    public class NewsRepository : INewsRepository
    {
        //private readonly ISearchServiceRepository searchServiceRepository;

        //public NewsRepository(ISearchServiceRepository searchServiceRepository)
        //{
        //    this.searchServiceRepository = searchServiceRepository;
        //}
        //public IEnumerable<Item> Get(Item contextItem)
        //{
        //    if (contextItem == null)
        //    {
        //        throw new ArgumentNullException(nameof(contextItem));
        //    }
        //    if (!contextItem.DescendsFrom(Templates.NewsFolder.ID))
        //    {
        //        throw new ArgumentException("Item must derive from NewsFolder", nameof(contextItem));
        //    }

        //    var searchService = this.searchServiceRepository.Get(new SearchSettingsBase { Templates = new[] { Templates.NewsArticle.ID } });
        //    searchService.Settings.Root = contextItem;
        //    //TODO: Refactor for scalability
        //    var results = searchService.FindAll();
        //    return results.Results.Select(x => x.Item).Where(x => x != null).OrderByDescending(i => i[Templates.NewsArticle.Fields.Date]);
        //}

        //public IEnumerable<Item> GetLatest(Item contextItem, int count)
        //{
        //    //TODO: Refactor for scalability
        //    return this.Get(contextItem).Take(count);
        //}

        public NewsListVM GetAll()
        {
            var database = Sitecore.Context.Database;

            var itemNews = database.GetItem(new ID("{F18D3E06-AFA1-47E0-83CA-030B0DF3F6C4}"));
            var fullPath = itemNews.Paths.FullPath;

            string query = $"/{fullPath}/*[@@templatename='NewsTemplate']";
            var searchResults = database.SelectItems(query);


            var viewModel = new NewsListVM()
            {
                NewsList = searchResults.Select(i =>
                    new NewsVM
                    {
                        Item = i,
                    }).ToList()
            };

            // Do sth with item
            return viewModel;
        }
    }
}