namespace sitecorepro3.Foundation.Indexing.Models
{
    using System.Collections.Generic;

    public interface IQueryFacet
    {
        string Title { get; set; }
        string FieldName { get; set; }
        string ViewName { get; set; }
    }
}