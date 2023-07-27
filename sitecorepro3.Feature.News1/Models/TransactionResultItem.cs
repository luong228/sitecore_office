using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace sitecorepro3.Feature.Transaction.Models
{
    public class TransactionResultItem : SearchResultItem
    {
        [IndexField("title_t")]
        public string Title { get; set; }
        [IndexField("description_t")]
        public string Descriptinon { get; set; }
        [IndexField("price_tf")]
        public float Price { get; set; }
        [IndexField("country_t")]
        public string Country { get; set; }
        [IndexField("state_t")]
        public string State { get; set; }
        [IndexField("postcode_t")]
        public string PostCode { get; set; }
        [IndexField("yearofconstruction_tf")]
        public float YearOfConstruction { get; set; }
        [IndexField("floorofproperty_tf")]
        public float FloorOfProperty { get; set; }
        [IndexField("numberofbedroom_tf")]
        public float NumberOfBedroom { get; set; }
        [IndexField("numberoffloors_tf")]
        public float NumberOfFloor { get; set; }
        [IndexField("area_t")]
        public string Area { get; set; }
        [IndexField("numberofbathroom_tf")]
        public float NumberOfBathRoom{ get; set; }
        [IndexField("numberofparking_t")]
        public string NumberOfParking { get; set; }
        [IndexField("cellarno_tf")]
        public float CellarNo { get; set; }
        [IndexField("propertycondition_t")]
        public string PropertyCondition { get; set; }
        [IndexField("city_t")]
        public string City { get; set; }
        [IndexField("categoryname_s")]
        public string CategoryName { get; set; }
        [IndexField("typelist_s")]
        public string Type { get; set; }
        [IndexField("_fullpath")]
        public string FullPath { get; set; }
    }
}