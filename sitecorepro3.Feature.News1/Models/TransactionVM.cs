using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Web.UI.WebControls;

namespace sitecorepro3.Feature.Transaction.Models
{
    public class TransactionVM
    {

        public Item Item { get; set; }
        public MvcHtmlString Title
        {
            get { return GetFieldValue("Title"); }
        }
        public MvcHtmlString Description
        {
            get { return GetFieldValue("Description"); }
        }
        public MvcHtmlString Price
        {
            get { return GetFieldValue("Price"); }
        }
        public MvcHtmlString Country
        {
            get { return GetFieldValue("Country"); }
        }
        public MvcHtmlString State
        {
            get { return GetFieldValue("State"); }
        }
        public MvcHtmlString PostCode
        {
            get { return GetFieldValue("PostCode"); }
        }
        public MvcHtmlString YearOfConstruction
        {
            get { return GetFieldValue("YearOfConstruction"); }
        }
        public MvcHtmlString NumberOfFloors
        {
            get { return GetFieldValue("NumberOfFloors"); }
        }
        public MvcHtmlString NumberOfProperty
        {
            get { return GetFieldValue("FloorOfProperty"); }
        }
        public MvcHtmlString NumberOfBathroom
        {
            get { return GetFieldValue("NumberOfBathroom"); }
        }
        public MvcHtmlString Area
        {
            get { return GetFieldValue("Area"); }
        }
        public MvcHtmlString NumberOfBedroom
        {
            get { return GetFieldValue("NumberOfBedroom"); }
        }
        public MvcHtmlString NumberOfParking
        {
            get { return GetFieldValue("NumberOfParking"); }
        }
        public MvcHtmlString CellarNo
        {
            get { return GetFieldValue("CellarNo"); }
        }
        public MvcHtmlString City
        {
            get { return GetFieldValue("City"); }
        }
        public MvcHtmlString PropertyCondition
        {
            get { return GetFieldValue("PropertyCondition"); }
        }
        public string Type
        {
            get
            {
                var typeIds = GetFieldValue("Type").ToString();
                var IdList = typeIds.Split('|');

                var types = "";
                foreach (var s in IdList)
                {
                    var cateItem = Context.Database.GetItem(new ID(s));
                    types += cateItem.Name + " ";

                }
                return types;

            }
        }
        public string Category
        {
            get
            {
                var cateId = GetFieldValue("Category").ToString(); 
                var cateItem = Context.Database.GetItem(new ID(cateId));
                return cateItem.Name;

            }
        }
        public string GetUrl
        {
            get
            {
                var language = Sitecore.Context.Language;
                UrlOptions options = new UrlOptions();
                options.AlwaysIncludeServerUrl = false; // Set to false to generate a relative URL
                options.Language = language;
                options.EmbedLanguage(language);
                options.LanguageEmbedding = LanguageEmbedding.Always;
                return Sitecore.Links.LinkManager.GetItemUrl(Item, options);

            }
        }

        public MvcHtmlString GetFieldValue(string fieldName)
        {
            return new MvcHtmlString(FieldRenderer.Render(Item, fieldName));
        }
    }
}