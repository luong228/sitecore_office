using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using sitecorepro3.Feature.Transaction.Models;

namespace sitecorepro3.Feature.Transaction.Services
{
    public static class SearchHelper
    {
        public static TransactionResultItemList GetAllTransaction()
        {
            var transacIndex = ContentSearchManager.GetIndex("sc102_transaction_index");


            using (var tranContext = transacIndex.CreateSearchContext())
            {

                var query = tranContext.GetQueryable<TransactionResultItem>();
                var result = query.ToList();
                var transacList = new TransactionResultItemList();

                transacList.TransactionList.AddRange(result);
                var name = transacList.TransactionList.FirstOrDefault().CategoryName;

                return transacList;
            }
        }
        public static TransactionResultItemList GetTransactionQuery(string queryString)
        {
            var transacIndex = ContentSearchManager.GetIndex("sc102_transaction_index");


            using (var tranContext = transacIndex.CreateSearchContext())
            {
         

                var filterPredicate = PredicateBuilder.True<TransactionResultItem>();
                filterPredicate = filterPredicate
                    .Or(item => item.Title.Contains(queryString)).Boost(5)
                    .Or(item1 => item1.Descriptinon.Contains(queryString)).Boost(4)
                        .Or(item2 => item2.Type.Contains(queryString)).Boost(3);

        

                // Fetch results from the search index using the predicate
                var results = tranContext.GetQueryable<TransactionResultItem>().Filter(filterPredicate);




                //var result = query.ToList();
                var transacList = new TransactionResultItemList();

               
                transacList.TransactionList.AddRange(results);

                return transacList;
            }
        }
    }
}