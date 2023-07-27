
namespace sitecorepro3.Feature.News
{
    using Sitecore.Data;
    public struct Templates
    {
        public struct NewsArticle
        {
            public static readonly ID ID = new ID("{2F962B30-4780-4F4C-AEC6-35FA09741A4F}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{F9426471-36AD-4B01-9B0F-CFA0AF6A84CA}");
                public const string Title_FieldName = "NewsTitle";

                public static readonly ID Image = new ID("{033E7A25-AC65-4A60-A76D-D78D04442DCA}");

                public static readonly ID Date = new ID("{A362D3B0-35F3-4BAC-81CB-4E6204E76378}");

                public static readonly ID Summary = new ID("{BDED4CAE-3AAB-48DF-956B-B43BEC312A76}");
                public const string Summary_FieldName = "NewsSummary";

                public static readonly ID Body = new ID("{ACBB6A4A-12F9-4BDE-85A7-0BEDD70F4AF0}");
                public const string Body_FieldName = "NewsBody";
            }
        }

        public struct NewsFolder
        {
            public static readonly ID ID = new ID("{F18D3E06-AFA1-47E0-83CA-030B0DF3F6C4}");
        }
    }
}