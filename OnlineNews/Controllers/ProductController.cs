using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineNews.Data;
using OnlineNews.Middleware;
using OnlineNews.Models;
using OnlineNews.Models.Database;
using OnlineNews.Models.ViewModels;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace OnlineNews.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        // List of all the products
        public IActionResult Index()
        {
            var cartItems = HttpContext.Session.GetObject<List<CartItem>>("CartItems") ?? new List<CartItem>();
            ViewData["CartItemCount"] = cartItems.Select(m => m.Quantity).Sum();
            return RedirectToAction("AllProducts", "Product");
        }
        public IActionResult AllProducts()
        {
            var products = _db.Products.Where(p => p.IsAvailable).ToList();
            return View(products);
        }
        // View the product details
        public async Task<IActionResult> Details(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [Authorize(Roles = "Admin, Editor,Writer")]
        // Add a new product 
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [Authorize(Roles = "Admin, Editor,Writer")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(product);
                    await _db.SaveChangesAsync();
                }
                catch
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [Authorize(Roles = "Admin,Editor,Writer")]
        public IActionResult Delete(int id)
        {
            var data = _db.Products.FirstOrDefault(x => x.Id == id);
            _db.Products.Remove(data);
            _db.SaveChanges();

            return RedirectToAction("Index", "Product");
        }
        private bool ProductExists(int id)
        {
            return _db.Products.Any(e => e.Id == id);
        }
        public IActionResult SearchResult(string searchitem)
        {
            if (string.IsNullOrEmpty(searchitem))
            {
                return View(new List<Product>());
            }
            var productList = _db.Products.AsQueryable();
            productList = productList.Where(p =>
                p.Name.Contains(searchitem) ||
                p.Price.ToString().Contains(searchitem) ||
                p.Category.Contains(searchitem) ||
                p.Description.Contains(searchitem)

            );
            var products = productList.ToList();
            if (!products.Any())
            {
                ViewBag.msg = $"No results found for '{searchitem}'.";
            }
            else
            {
                ViewBag.msg = searchitem;
            }
            return View(products);
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
