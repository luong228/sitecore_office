using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;
using sitecorepro3.Feature.News.Models;

namespace sitecorepro3.Feature.News.Repositories
{
    public interface INewsRepository
    {
        //IEnumerable<Item> Get(Item contextItem);
        //IEnumerable<Item> GetLatest(Item contextItem, int count);
        NewsListVM GetAll();
    }
}
