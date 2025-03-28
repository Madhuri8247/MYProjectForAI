using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using OnlineNews.Models.ViewModels;
using OnlineNews.Models;

namespace OnlineNews.Services
{
    public interface ICartService
    {
        public CartViewModel GetCartProducts(List<CartItem> cartItems);
        public void AddToCart(List<CartItem> cartItems, OnlineNews.Models.Product product);
        public void RemoveFromCart(List<CartItem> cartItems, int productId);
        public void LowerQuantity(List<CartItem> cartItems, int productId);
    }
}
