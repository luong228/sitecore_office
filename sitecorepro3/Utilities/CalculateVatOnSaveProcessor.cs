using System;
using Sitecore.Pipelines.ItemProvider.SaveItem;
using Convert = System.Convert;

namespace sitecorepro3.Project.sitecorepro3.Utilities
{
    public class CalculateVatOnSaveProcessor
    {
        public void Process(SaveItemArgs args)
        {
            Sitecore.Diagnostics.Assert.ArgumentNotNull(args, "args");
            if (args.Item == null)
            { return; }
            Sitecore.Data.ID itemId = Sitecore.Data.ID.Parse("{9CEC698F-EFE1-434C-9591-544E8BB82075}");
            if (args.Item.TemplateID != itemId) { return; }

            var priceField = args.Item.Fields["Price"].Value;
            if (string.IsNullOrEmpty(priceField))
            {
                return;
            }
            var price = Convert.ToDouble(priceField);
            var currentVat = args.Item.Fields["VAT"].Value;

            double vat = 0;
            if (!String.IsNullOrEmpty(currentVat))
            {
                vat = Convert.ToDouble(args.Item.Fields["VAT"].Value);
            }

            
            if (Math.Abs(price * 0.1 - vat) < 0.01)
                return;

            vat = price * 0.1;

            args.Item.Editing.BeginEdit();
            args.Item.Fields["VAT"].Value = vat.ToString();
            args.Item.Editing.EndEdit();


        }
    }
}