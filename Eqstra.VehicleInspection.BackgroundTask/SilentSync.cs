using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Eqstra.VehicleInspection.BackgroundTask
{
    public sealed class SilentSync : IBackgroundTask
    {
       private const string worldWeatherAPI = "http://api.worldweatheronline.com/free/v1/weather.ashx?format=json&num_of_days=1&key=n5v69mv2e2kmyq93u2m494wt&q=";
      async  public void Run(IBackgroundTaskInstance taskInstance)
        {

            var request = (HttpWebRequest)WebRequest.Create(worldWeatherAPI + "hyderabad,India");
            var response = (HttpWebResponse)await request.GetResponseAsync();
            JsonSerializer serializer = new JsonSerializer();
            var reader = new JsonTextReader(new StreamReader(response.GetResponseStream()));
            var weatherOjb = JObject.Parse(serializer.Deserialize(reader).ToString());
            var currentCondition = weatherOjb["data"]["current_condition"];
            var weather = (from item in currentCondition
                           select new
                           {
                               CloudCover = item["cloudcover"],
                               Humidity = item["humidity"],
                               precipMM = item["precipMM"],
                               temp_C = item["item_C"],
                               temp_F = item["temp_F"],
                               weatherIconUrl = item["weatherIconUrl"][0]["value"].ToString(),
                               weatherDesc = item["weatherDesc"][0]["value"].ToString(),
                           }).First();            
            
            
        }
    }
}
