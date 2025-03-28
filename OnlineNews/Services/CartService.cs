using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using OnlineNews.Models.ViewModels;
using OnlineNews.Models;

namespace OnlineNews.Services
{
    public class CartService : ICartService
    {

        public CartViewModel GetCartProducts(List<CartItem> cartItems)
        {
            var cartViewModel = new CartViewModel
            {
                CartItem = cartItems,
                SubTotalPrice = cartItems.Sum(item => item.product.Price * item.Quantity)
            };
            return cartViewModel;
        }

        public void AddToCart(List<CartItem> cartItems, Models.Product product)
        {
            if (cartItems == null)
            {
                cartItems = new List<CartItem>();
            }

            var cartItem = cartItems.FirstOrDefault(item => item.product.Id == product.Id);
            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cartItems.Add(new CartItem
                {
                    product = product,
                    Quantity = 1
                });
            }
        }

        public void RemoveFromCart(List<CartItem> cartItems, int productId)
        {
            var cartItem = cartItems.FirstOrDefault(item => item.product.Id == productId);
            if (cartItem != null)
            {
                cartItems.Remove(cartItem);
            }
        }
        public void LowerQuantity(List<CartItem> cartItems, int productId)
        {
            var cartItem = cartItems.FirstOrDefault(item => item.product.Id == productId);
            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                }
                else
                {
                    cartItems.Remove(cartItem);
                }
            }
        }
    }
}
