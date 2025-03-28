using OnlineNews.Services;
using OnlineNews.Models;
using OnlineNews.Data;

namespace OnlineNews.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext  _db;

        public OrderService(ApplicationDbContext db)
        {
            _db = db;
        }
       public List <Order> AdminOrder()
       {
            var allOrdersAdmin = _db.Orders
                .OrderByDescending(c=>c.OrderDate).ToList();
            return allOrdersAdmin;
       }
        public List<Order> LatestFiveOrders()
        {
            var latestOrders = _db.Orders.OrderBy(d => d.OrderDate).Take(5).ToList();

            return latestOrders;

        }
    }
}
