using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace sitecorepro3.Feature.Transaction.Models
{
    public class TransactionResultItemList
    {
        public TransactionResultItemList()
        {
            TransactionList = new List<TransactionResultItem>();
        }
        public List<TransactionResultItem> TransactionList { get; set; }
        public List<Type> TypeList { get; set; }
    }
}