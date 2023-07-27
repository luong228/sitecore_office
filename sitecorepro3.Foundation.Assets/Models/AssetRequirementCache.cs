﻿using Sitecore.Caching;
using Sitecore.Data;

namespace sitecorepro3.Foundation.Assets.Models
{
    internal class AssetRequirementCache : CustomCache
    {
        public AssetRequirementCache(long maxSize) : base("Sitecore.Foundation.AssetRequirements", maxSize)
        {
        }

        public AssetRequirementList Get(ID cacheKey)
        {
            return (AssetRequirementList)this.GetObject(cacheKey.ToString());
        }

        public void Set(ID cacheKey, AssetRequirementList requirementList)
        {
            this.SetObject(cacheKey.ToString(), requirementList);
        }
    }
}