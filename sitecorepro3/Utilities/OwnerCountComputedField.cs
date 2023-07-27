using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace sitecorepro3.Project.sitecorepro3.Utilities
{
    public class OwnerCountComputedField : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            Item item = indexable as SitecoreIndexableItem;
            if (item == null)
                return null;
            
            var ownerCount = GetOwnerCount(item);

            return ownerCount;
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        private string GetOwnerCount(Item item)
        {
            var ownersCount = 0;
            if (item != null)
            {
                var owners = item.GetChildren();
               
                foreach (Item owner in owners)
                {
                    if (owner.TemplateID == Sitecore.Data.ID.Parse("{1369CB5C-4252-4B24-88AA-DC82BC2A3370}"))
                        ownersCount++;
                }
                
            }
            return string.Join("|", ownersCount);
        }
        
    }
}