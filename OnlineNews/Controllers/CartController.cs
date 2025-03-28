using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using OnlineNews.Data;
using OnlineNews.Middleware;
using OnlineNews.Models;
using OnlineNews.Models.ViewModels;
using OnlineNews.Services;
using Product = OnlineNews.Models.Product;

namespace OnlineNews.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ApplicationDbContext db, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var cartItems = HttpContext.Session.GetObject<List<CartItem>>("CartItems") ?? new List<CartItem>();
            if (cartItems == null)
            {
                _logger.LogInformation("Loading Cart/Index view.");
                return View();
            }
            CartViewModel cart = _cartService.GetCartProducts(cartItems);
            ViewData["CartItemCount"] = cartItems.Select(m => m.Quantity).Sum();
            return View(cart);
        }

        public IActionResult AddProductToCart(int id)
        {
            var product = _db.Products.FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction(nameof(Index));  // Redirect to a default page if product doesn't exist
            }

            var cartItems = HttpContext.Session.GetObject<List<CartItem>>("CartItems");

            // Ensure cartItems is initialized
            if (cartItems == null)
            {
                cartItems = new List<CartItem>();
            }

            _cartService.AddToCart(cartItems, product);  // Add to cart logic
            HttpContext.Session.SetObject<List<CartItem>>("CartItems", cartItems);  // Save updated cart

            TempData["SuccessMessage"] = "Item added to cart successfully";

            // Handle referrer and redirect
            var refererUrl = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(refererUrl))
            {
                return Redirect(refererUrl);  // Redirect back to the page the user was on
            }

            // If referer is not available, fallback to a default page (Home Index)
            return RedirectToAction(nameof(Index));
        }


        public IActionResult RemoveProductFromCart(int id)
        {
            var movie = _db.Products.FirstOrDefault(m => m.Id == id);
            if (movie != null)
            {
                var cartItems = HttpContext.Session.GetObject<List<CartItem>>("CartItems");
                _cartService.RemoveFromCart(cartItems, id);
                HttpContext.Session.SetObject<List<CartItem>>("CartItems", cartItems);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult LowerQuantity(int id)
        {
            var movie = _db.Products.FirstOrDefault(m => m.Id == id);
            if (movie != null)
            {
                var cartItems = HttpContext.Session.GetObject<List<CartItem>>("CartItems");
                _cartService.LowerQuantity(cartItems, id);
                HttpContext.Session.SetObject<List<CartItem>>("CartItems", cartItems);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult EmptyCart()
        {
            HttpContext.Session.Remove("CartItems");
            return View();
        }

        [HttpPost]
        public IActionResult ProceedToCheckout(CartViewModel cartViewModel)
        {
            var cartItems = HttpContext.Session.GetObject<List<CartItem>>("CartItems");

            if (cartItems == null || !cartItems.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            var customer = cartViewModel.Customer;

            var order = new Order
            {
                Customer = customer,
                OrderDate = DateTime.Now
            };

            foreach (var cartItem in cartItems)
            {
                var orderRow = new OrderRow
                {
                    ProductId = cartItem.product.Id,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.product.Price
                };
                order.OrderRows.Add(orderRow);
            }

            _db.Orders.Add(order);
            _db.SaveChanges();
            HttpContext.Session.Remove("CartItems");
            return RedirectToAction("Checkout", new { orderId = order.Id });
        }

        [HttpGet]
        public JsonResult GetCustomerByEmail(string email)
        {
            var customer = _db.Customers
      .Where(c => c.EmailAddress == email)
      .FirstOrDefault();

            if (customer != null)
            {
                var customerData = new
                {
                    firstName = customer.FirstNameBillingAddress,
                    lastName = customer.LastNameBillingAddress,
                    billingAddress = customer.BillingAddress,
                    billingZip = customer.BillingZip,
                    billingCity = customer.BillingCity
                };
                return Json(customerData);
            }

            return Json(null);
        }

        public IActionResult Checkout()
        {

            return View();
        }
    }
}
