using OnlineNews.Models.API;

namespace OnlineNews.Services
{
    public interface IRequestService
    {
        Task<SpotPriceNow> GetData();
        Task<WeatherForecast> GetWeatherByCityNameAsync(string cityName);
        Task<Businessprice> GetPrices();
    }
}
