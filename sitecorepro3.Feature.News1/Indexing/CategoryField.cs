using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using System.Collections.Generic;
using System;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;

namespace sitecorepro3.Feature.Transaction.Indexing
{
    public class CategoryField : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable p_Indexable)
        {
            IIndexableDataField categoryField = p_Indexable.GetFieldByName("Category");
            if (categoryField != null)
            {
                try
                {
                    var value = categoryField.Value.ToString();
                    var db = Factory.GetDatabase("master");

                    Item cate = db.GetItem(new ID(value));

                    return cate.Name;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return null;
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
}