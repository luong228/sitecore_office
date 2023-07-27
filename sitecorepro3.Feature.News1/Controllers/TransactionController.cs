
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using sitecorepro3.Feature.Transaction.Models;
using sitecorepro3.Foundation.Indexing.Repositories;
using sitecorepro3.Foundation.SitecoreExtensions.Extensions;
using SearchHelper = sitecorepro3.Feature.Transaction.Services.SearchHelper;
using Type = sitecorepro3.Feature.Transaction.Models.Type;

namespace sitecorepro3.Feature.Transaction.Controllers
{
    public class TransactionController : Controller
    {
        public TransactionController()
        {
        }


        public ActionResult TransactionSearch()
        {
            string queryString = Request.QueryString["query"];
            var transacList = new TransactionResultItemList();
            if (string.IsNullOrEmpty(queryString))
            {
                transacList = SearchHelper.GetAllTransaction();
            }
            else
            {
                transacList = SearchHelper.GetTransactionQuery(queryString);
            }
            

            var database = Sitecore.Context.Database;
            var typePath = "/sitecore/content/home/office/TransactionType";

            string queryType = $"/{typePath}/*[@@templatename='Transaction Type template']";

            var typeList = database.SelectItems(queryType);
            transacList.TypeList = typeList.Select(i => 
                new Type()
                {
                    Item = i

                }).ToList();
            return View(transacList);
        }

        public ActionResult TransactionList()
        {
            var database = Sitecore.Context.Database;

            string query = $"/{Sitecore.Context.Item.Paths.FullPath}/*[@@templatename='Transaction Template']";
            var searchResults = database.SelectItems(query);

            var typePath = "/sitecore/content/home/office/TransactionCategories";
            string queryType = $"/{typePath}/*[@@templatename='Transaction Type template']";

            var typeList = database.SelectItems(queryType);
            var viewModel = new TransactionListVM()
            {
                TransactionList = searchResults.Select(i =>
                    new TransactionVM()
                    {
                        Item = i,
                    }).ToList(),
                TypeList = typeList.Select(i =>
                    new Type()
                    {
                        Item = i
                    }
                    ).ToList(),

            };
            return this.View("TransactionList", viewModel);
        }
        public ActionResult TransactionDetail()
        {
            Item contextItem = Sitecore.Context.Item;

            var transaction = new TransactionVM()
            {
                Item = contextItem
            };
            return View(transaction);
        }
    }
}