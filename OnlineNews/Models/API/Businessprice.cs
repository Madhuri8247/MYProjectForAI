
namespace OnlineNews.Models.API
{
    public class Businessprice
    {
        public Top10[] top10 { get; set; }
    }
    public class Top10
    {
        public string name { get; set; }
        public string symbol { get; set; }
        public float close { get; set; }
        public float prevClose { get; set; }
        public float percentChange { get; set; }
    }
}
