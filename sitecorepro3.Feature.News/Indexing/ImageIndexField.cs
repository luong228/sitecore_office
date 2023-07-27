using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;

namespace sitecorepro3.Feature.News.Indexing
{
    public class ImageIndexField : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            Assert.ArgumentNotNull(indexable, "sc102_news_index");
            var indexableItem = indexable as SitecoreIndexableItem;

            if (indexableItem == null)
            {
                Log.Warn(string.Format("{0} : unsupported IIndexable type : {1}", this, indexable.GetType()), this);
                return null;
            }

            ImageField img = indexableItem.Item.Fields["Image"];

            return img == null || img.MediaItem == null ? null : MediaManager.GetMediaUrl(img.MediaItem);
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
}