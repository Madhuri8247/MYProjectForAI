using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace OnlineNews.Models.API
{
    public class WeatherForecast
    {
        [JsonProperty("summary")]
        public string summary { get; set; }
        public string city { get; set; }

        [JsonProperty("lang")]
        public string language { get; set; }
        public int temperatureC { get; set; }
        public int temperatureF { get; set; }
        public int humidity { get; set; }
        public int windSpeed { get; set; }
        public DateTime date { get; set; }
        public int unixTime { get; set; }
        public Icon icon { get; set; }
    }
}
public class Icon
{
    public string url { get; set; }
    public string code { get; set; }
}
