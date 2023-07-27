using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sitecorepro3.Feature.Transaction.Models
{
    public class TransactionListVM
    {
        public TransactionListVM()
        {
            TransactionList = new List<TransactionVM>();
            TypeList = new List<Type>();
        }
    public List<TransactionVM> TransactionList { get; set; }
    public List<Type> TypeList{ get; set; }
    }
}