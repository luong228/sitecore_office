using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;
using sitecorepro3.Project.sitecorepro3.Areas.Product.Models;
using Image = Sitecore.Web.UI.XslControls.Image;
using Item = Sitecore.Data.Items.Item;

namespace sitecorepro3.Project.sitecorepro3.Areas.Product.Controllers
{
    public class ProductList2Controller : Controller
    {
        

        
        // GET: Product/ProductList2
        [HttpGet]
        public ActionResult Index()
        {

            //var database = Sitecore.Context.Database;
            //var item = database.GetItem(new Sitecore.Data.ID("{B0F3D1FB-E6BB-4C0C-93EA-7A5E2647D2F0}"));
            ////var itemId = item.ID.ToString();
            //var slideIds = Sitecore.Data.ID.ParseArray(item["productList"]);
            //var viewModel = new ProductListVM
            //{
            //    ProductList =
            //        slideIds.Select(i =>
            //            new ProductVM
            //            {
            //                Item = item.Database.GetItem(i)
            //            }).ToList()
            //};


            //Sitecore.Data.Items.Item contextItem = Sitecore.Context.Item;
            //var productIds = Sitecore.Data.ID.ParseArray(contextItem["productList"]);

            //MultilistField treelistField = (MultilistField)contextItem.Fields["productList"];
            //Item[] selectedItems = treelistField.GetItems();

            //var viewModel = new ProductListVM
            //{
            //    ProductList = selectedItems.Select(i =>
            //        new ProductVM
            //        {
            //            Item = i,
            //        }).ToList()
            //};

            var database = Sitecore.Context.Database;
            //var products = database.GetItem("{B0F3D1FB-E6BB-4C0C-93EA-7A5E2647D2F0}" ).Children;
            // Get current context way
            //var productChildren = Sitecore.Context.Item.GetChildren();


            string query = $"/{Sitecore.Context.Item.Paths.FullPath}/*[@@templatename='Product']";
            var searchResults = database.SelectItems(query);


            var viewModel = new ProductListVM
            {
                ProductList = searchResults.Select(i =>
                    new ProductVM
                    {
                        Item = i,
                    }).ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            string searchString = formCollection["searchString"].ToString();
            var database = Sitecore.Context.Database;// master db

            string query = $"{Sitecore.Context.Item.Paths.FullPath}/*[@@templatename='Product' and @Name='{searchString}']";
            var searchResults = database.SelectItems(query);

            var searchTest = global::sitecorepro3.Project.sitecorepro3.Utilities.SearchHelper.SearchByProduct("owner", "23", null);

            if (searchResults.Length == 0)
            {
                TempData["NotFound"] = "Not Found";
                return View("Index", new ProductListVM());
            }
            var viewModel = new ProductListVM
            {
                ProductList = searchResults.Select(i =>
                    new ProductVM
                    {
                        Item = i,
                    }).ToList()
            };
            return View("Index", viewModel);

        }

        public ActionResult Details()
        {
            Item contextItem = Sitecore.Context.Item;

            var product = new ProductVM()
            {
                Item = contextItem
            };
            return View(product); 
        }
        public ActionResult EditImage()
        {
            HyperLink HyperLink1 = new HyperLink();
            Image Image1 = new Image();
            LinkField linkField = Sitecore.Context.Item.Fields["General Link Field"];
            if (linkField != null)
            {
                if (linkField.LinkType == "external" || linkField.LinkType == "anchor" || linkField.LinkType == "mailto" || linkField.LinkType == "javascript")
                {
                    HyperLink1.Text = linkField.Text;
                    HyperLink1.NavigateUrl = linkField.Url;
                }
                if (linkField.LinkType == "internal")
                {
                    HyperLink1.Text = linkField.Text;
                    HyperLink1.NavigateUrl = Sitecore.Links.LinkManager.GetItemUrl(linkField.TargetItem);
                }
                else if (linkField.LinkType == "media")
                {
                    //Image1.ImageUrl = Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(linkField.TargetItem));
                }
            }

            return View("Index"); 
        }
        public ActionResult GetPromotion()
        {
            var contextItem = RenderingContext.Current.Rendering.Item;


            var promotion = new PromotionVM()
            {
                Item = contextItem
            };
            return View(promotion);
        }
        public string GetUrl(Sitecore.Data.Items.Item item)
        {
            return Sitecore.Links.LinkManager.GetItemUrl(item);
        }
        public string GetMediaUrl(Sitecore.Data.Items.Item item)
        {
            return Sitecore.Resources.Media.MediaManager.GetMediaUrl(item);
        }

    }
}
//@if(IsPost && !string.IsNullOrEmpty(Request.Form["searchString"]))
//{
//    var searchString = Request.Form["searchString"];
//    var searchResults = Html.Sitecore().Controller("ProductList2", "Search");
//    @*@Html.RenderPartial("../ProductSearch/SearchResults", searchResults); *@
//}