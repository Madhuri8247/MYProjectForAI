using Newtonsoft.Json;

namespace OnlineNews.Models.API
{
    public class SpotPriceNow
    {
        public string date { get; set; }
        public SECategory[] SE1 { get; set; }
        public SECategory[] SE2 { get; set; }
        public SECategory[] SE3 { get; set; }
        public SECategory[] SE4 { get; set; }
    }
}


public class SECategory
{
    public int hour { get; set; }
    public float price_eur { get; set; }
    public float price_sek { get; set; }
    public int kmeans { get; set; }
}


