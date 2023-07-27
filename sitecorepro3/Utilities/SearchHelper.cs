using System.Collections.Generic;
using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;

namespace sitecorepro3.Project.sitecorepro3.Utilities
{
    public static class SearchHelper
    {
        public static List<ProductSearchResultItem> SearchByProduct(string? ownerName, string? name, string? price)
        {

            var ownerIndex = ContentSearchManager.GetIndex("sc102_owner_index");


            using (var ownerContext = ownerIndex.CreateSearchContext())
            {

                var ownerQuery = ownerContext.GetQueryable<OwnerSearchResultItem>()
                    .Where(item => item.OwnerName.Like(ownerName));

                var owners = ownerQuery.ToList();

                var index = ContentSearchManager.GetIndex("sc102_master_products_index");
                var productList = new List<ProductSearchResultItem>();

                using (var context = index.CreateSearchContext())
                {
                    var resultTest = context.GetQueryable<ProductSearchResultItem>().ToList();
                    foreach (var owner in owners)
                    {
                        
                        var query = context.GetQueryable<ProductSearchResultItem>()
                            .Where(item => item.OwnerName.Like(owner.Name))
                            .Where(item => item.ProductName.Like(name));
                        var result = query.ToList();
                        productList.AddRange(result);
                    }


                    return productList;
                }
            }
        }
    }
}