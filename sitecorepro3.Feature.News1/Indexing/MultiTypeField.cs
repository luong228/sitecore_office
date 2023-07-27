using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using System.Collections.Generic;
using System;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Configuration;

namespace sitecorepro3.Feature.Transaction.Indexing
{
    public class MultiTypeField : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable p_Indexable)
        {
            IIndexableDataField multiValueField = p_Indexable.GetFieldByName("Type");
            if (multiValueField != null)
            {
                try
                {
                    // The raw value of a Multilist field looks like this:
                    // {F90052A5-B4E6-4E6D-9812-1E1B88A6FCEA}|{F0D16EEE-3A05-4E43-A082-795A32B873C0}
                    // So we split it at the "|" character to retrieve the GUIDs.
                    string[] referencedItemGuids = multiValueField.Value.ToString().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    List<string> referencedItemNames = new List<string>();
                    foreach (string referencedItemGuid in referencedItemGuids)
                    {
                        // Then we retrieve the name of each item referenced by these GUIDs.
                        var db1 = Factory.GetDatabase("master");
                        Item item = db1.GetItem(new ID(referencedItemGuid));
                        referencedItemNames.Add(item.Name);
                    }
                    // Finally, we join all those item names using the default separator (;) to get a properly-formatted field.
                    return String.Join(";", referencedItemNames.ToArray());
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