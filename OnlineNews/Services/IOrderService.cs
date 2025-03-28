using OnlineNews.Models;

namespace OnlineNews.Services
{
    public interface IOrderService
    {
        public List<Order> SpecificCustomerOrder();
        public List<Order> LatestFiveOrders();
    }
}
