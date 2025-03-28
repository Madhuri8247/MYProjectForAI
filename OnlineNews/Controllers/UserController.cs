using Microsoft.AspNetCore.Mvc;
using OnlineNews.Models.Database;
using OnlineNews.Models.ViewModels;
using OnlineNews.Service;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;  // Add this for Role-based checks

namespace OnlineNews.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly UserManager<User> _userManager;  // Inject UserManager to access user roles

        public UserController(IUserService userService, ISubscriptionService subscriptionService, UserManager<User> userManager)
        {
            _userService = userService;
            _subscriptionService = subscriptionService;
            _userManager = userManager;  // Initialize the UserManager
        }

        [HttpGet]
        public async Task<IActionResult> MyPage()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    TempData["Error"] = "User not found.";
                    return RedirectToAction("Index", "Home");
                }

                // Check if the user has roles like "Admin" or "Subscriber"
                var roles = await _userManager.GetRolesAsync(user);
                bool isRoleRestricted = roles.Contains("Admin") || roles.Contains("Subscriber") || roles.Contains("Editor") || roles.Contains("Writer");

                // If the user is not in a restricted role, fetch subscription data
                if (!isRoleRestricted)
                {
                    var subscription = await _subscriptionService.PaymentConfirmation(userId);

                    if (subscription == null || !subscription.PaymentComplete)
                    {
                        TempData["Error"] = "You don't have an active subscription.";
                        return RedirectToAction("Subscribe", "Subscription");
                    }

                    // If the user is allowed to see subscription info, set the model
                    var model = new UserPageViewModel
                    {
                        User = user,
                        Subscription = subscription
                    };

                    return View(model);
                }

                // If the user has restricted roles, display only the user info
                var modelRestricted = new UserPageViewModel
                {
                    User = user
                };

                return View(modelRestricted);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while fetching user data.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeSubscription(string subscriptionType)
        {
            if (string.IsNullOrEmpty(subscriptionType))
            {
                TempData["Error"] = "Subscription type is required.";
                return RedirectToAction("MyPage");
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Fetch the subscription type by name
                var subscriptionTypeEntity = await _subscriptionService.GetSubscriptionTypeByNameAsync(subscriptionType);

                if (subscriptionTypeEntity == null)
                {
                    TempData["Error"] = "Invalid subscription type.";
                    return RedirectToAction("MyPage");
                }

                // Attempt to change the subscription
                var success = await _subscriptionService.ChangeSubscriptionTypeAsync(userId, subscriptionTypeEntity);

                if (success)
                {
                    TempData["Message"] = "Subscription updated successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to update subscription. Please try again later.";
                }

                return RedirectToAction("MyPage");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while changing the subscription.";
                return RedirectToAction("MyPage");
            }
        }
    }
}
