using Microsoft.AspNetCore.Mvc;
using OnlineNews.Models.Database;
using OnlineNews.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineNews.Data;
using Microsoft.EntityFrameworkCore;


namespace OnlineNews.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;

        public SubscriptionController(ISubscriptionService subscriptionService, UserManager<User> userManager, ApplicationDbContext db)
        {
            _subscriptionService = subscriptionService;
            _userManager = userManager;
            _db = db;
        }

        [HttpGet]
        public IActionResult Subscribe()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(string subscriptionType)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Get the corresponding subscription type from the database using case-insensitive comparison.
            var subscriptionTypeEntity = await _db.SubscriptionTypes
                .FirstOrDefaultAsync(s => s.TypeName.ToLower() == subscriptionType.ToLower());

            if (subscriptionTypeEntity == null)
            {
                TempData["Error"] = "Invalid subscription type selected.";
                return RedirectToAction("Subscribe");
            }

            // Create the new subscription for the user
            Subscription newSubscription = new Subscription()
            {
                Subscriber = user,
                CreatedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddDays(30), // Adjust expiry based on your needs
                PaymentComplete = false,
                SubscriptionType = subscriptionTypeEntity
            };

            // Add the subscription to the database
            await _subscriptionService.AddSubscriptionAsync(newSubscription, subscriptionTypeEntity.TypeName);

            // Set the success message for the payment page
            TempData["Message"] = "Please complete the payment.";
            // Ensure other messages (like 'Error') are cleared
            TempData.Remove("Error");

            // Redirect to the DummyPayment page for payment processing
            return RedirectToAction("DummyPayment");
        }


        [HttpGet]
        public IActionResult DummyPayment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DummyPayment(string cardNumber, string expiryDate, string cvv)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("MyPage", "User");
            }

            var subscription = await _subscriptionService.PaymentConfirmation(userId);

            if (subscription != null)
            {
                subscription.PaymentComplete = true;
                await _db.SaveChangesAsync();  // Ensure changes are saved to the database
                TempData["Message"] = "Payment Successful! You are now an active subscriber.";
            }
            else
            {
                TempData["Error"] = "Failed to complete payment.";
            }

            return RedirectToAction("MyPage", "User");
        }


        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return View(user);
        }
        
        [HttpPost]
        public async Task<IActionResult> EditProfile(User updatedUser)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.Email = updatedUser.Email;
                user.DateofBirth = updatedUser.DateofBirth;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["Message"] = "Profile updated successfully!";
                    return RedirectToAction("MyPage", "User");
                }

                TempData["Error"] = "Failed to update profile.";
            }

            return View(updatedUser);
        }




        [Authorize]
        public async Task<IActionResult> SubscriptionDetails()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Get the subscription of the logged-in user
            var subscription = await _db.Subscriptions
                .Include(s => s.SubscriptionType) // Include the subscription type (Basic, Premium, etc.)
                .FirstOrDefaultAsync(s => s.Subscriber.Id == userId);

            if (subscription == null)
            {
                TempData["Error"] = "You have no active subscription.";
                return RedirectToAction("Subscribe");
            }

            return View(subscription);
        }

    }
}