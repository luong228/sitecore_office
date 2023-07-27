using Sitecore.Data;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using log4net.spi;
using Sitecore.Data.Items;

namespace sitecorepro3.Feature.CallApi.Tasks
{
    public class WeatherTask 
    {
        public  void Execute(Item[] items, Sitecore.Tasks.CommandItem commandItem,
            Sitecore.Tasks.ScheduleItem scheduleItem)
        {
            // Gọi API thời tiết từ Tomorrow.io
            string url = "https://api.tomorrow.io/v4/weather/realtime?location=ho%20chi%20minh&apikey=C1YPdLYHSFXOHKQ3NZZnIf0BseBSM6EF";
            WebClient client = new WebClient();
            string json = client.DownloadString(url);

            // Lấy giá trị nhiệt độ từ JSON
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            double temperature = data?.data.values?.temperature ?? 0;
            double windSpeed = data?.data.values?.windSpeed ?? 0;
            double humidity = data?.data.values?.humidity ?? 0;
            double uvIndex = data?.data.values?.uvIndex ?? 0;
            string time = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            string name = data?.location?.name ?? "Ho Chi Minh City, Vietnam";

            var content = $"Name: {name}\n" + 
                          $"Temperature: {temperature}\n" +
                          $"Wind Speed: {windSpeed}\n" +
                          $"Humidity: {humidity}\n" +
                          $"UV Index: {uvIndex}\n" +
                          $"Time: {time}\n";

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
        }
    }
}