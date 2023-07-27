using Sitecore.Data;
using sitecorepro3.Feature.News.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using sitecorepro3.Feature.News.Repositories;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using System.Net;
using Sitecore.Data.Items;

namespace sitecorepro3.Feature.News.Controllers
{
    public class NewsApiController : ApiController
    {
        private readonly INewsRepository _newsRepository;

       
        public NewsApiController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }
        // GET: NewsApi
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/news/getallnews")]
        public IHttpActionResult GetAllNews()
        {

            var news = _newsRepository.GetAll();
            var itemData = news.NewsList.Select(item => new
            {
                Id = item.Item.ID.ToString(),
                Title = item.Title.ToString(),
                Summary = item.Summary.ToString(),
                Body = item.Body.ToString(),
                ImageUrl = item.ImageUrl.ToString(),
                Date = item.Date.ToString(),
                Url = item.GetUrl.ToString(),
                Path = item.Item.Paths.FullPath.ToString()
            });
            Log.Info("Api Get All News has called", this);
            return Json(itemData);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/weather")]
        public IHttpActionResult GetWeather()
        {
            string url = "https://api.tomorrow.io/v4/weather/realtime?location=ho%20chi%20minh&apikey=C1YPdLYHSFXOHKQ3NZZnIf0BseBSM6EF";
            WebClient client = new WebClient();
            string json = client.DownloadString(url);

            // Lấy giá trị nhiệt độ từ JSON
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            double temperature = data?.data.values?.temperature ?? 0;
            double windSpeed = data?.data.values?.windSpeed ?? 0;
            double humidity = data?.data.values?.humidity ?? 0;
            double uvIndex = data?.data.values?.uvIndex ?? 0;

            var content = $"Temperature: {temperature}<br>" +
                          $"Wind Speed: {windSpeed}<br>" +
                          $"Humidity: {humidity}<br>" +
                          $"UV Index: {uvIndex}<br>";

                          // Lưu giá trị nhiệt độ vào một item Sitecore
            Database database = Sitecore.Data.Database.GetDatabase("master");
            var itemId = new ID("{EA056730-AA3C-4C2A-81DB-746AE35E6FDC}");
            Item item = database.GetItem(itemId);
            using (new SecurityDisabler())
            {
                item.Editing.BeginEdit();
                item.Fields["Text"].Value = content;
                item.Editing.EndEdit();
            }
            return Json(data);
        }
    }
}