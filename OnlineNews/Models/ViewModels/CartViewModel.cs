using OnlineNews.Models;
using System.ComponentModel.DataAnnotations;

namespace OnlineNews.Models.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> CartItem { get; set; } = new List<CartItem>();
        public decimal SubTotalPrice  { get; set; }
        public decimal ShippingCost { get; set; } = 0; // Shipping Cost - constant value
        public decimal Tax => SubTotalPrice * 0.25m; // Tax rate 25%
        public decimal TotalPrice => SubTotalPrice + ShippingCost; // Combined
        public Customer Customer { get; set; } = new Customer();
        public int OrderId { get; set; }
        public string Email { get; set; }
    }
    public class CartItem
    {
        public Product product { get; set; }
        public int Quantity { get; set; }
    }
    public class OrderSummaryViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }
        public List<OrderRow> OrderRows { get; set; } = new List<OrderRow>();
        public decimal TotalPrice { get; set; }
    }
}
