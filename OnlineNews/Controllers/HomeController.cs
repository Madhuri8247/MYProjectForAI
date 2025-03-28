using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineNews.Interfaces;
using OnlineNews.Models;
using OnlineNews.Models.API;
using OnlineNews.Service;
using OnlineNews.Services;
using System.Diagnostics;
using System.Security.Claims;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly IRequestService _requestService;
    private readonly IArticleService _articleService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ISubscriptionService _subscriptionService;
    public HomeController(ILogger<HomeController> logger, IUserService userService, IRequestService requestService, ISubscriptionService subscriptionService, IArticleService articleService, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _userService = userService;
        _requestService = requestService;
        _articleService = articleService;
        _httpContextAccessor = httpContextAccessor;
        _subscriptionService = subscriptionService;

    }
    public async Task<IActionResult> Index()
    {
        ViewBag.HasConsented = _articleService.HasConsented(_httpContextAccessor);
        var result = await _userService.AddEmployee();
        FrontPageViewModel obj = new FrontPageViewModel();
        obj.SomeLatestNews = _articleService.SomeLatestNews();
        obj.Mostpopular = _articleService.Mostpopular();
        obj.OneLatestNews = _articleService.OneLatestNews();
        obj.EditorsChoiceArticles = _articleService.EditorsChoice();
        return View(obj);
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult Claims()
    {
        var user = HttpContext.User;
        var claims = user.Claims;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (user.IsInRole("Admin"))
        {
            return NotFound("Deactivated");
        }
        return RedirectToAction("ListUsers");
    }
    public async Task<IActionResult> WeatherSearch(string city)
    {
        WeatherForecast weather = null;
        if (!string.IsNullOrEmpty(city))
        {
            weather = await _requestService.GetWeatherByCityNameAsync(city);
        }

        ViewData["City"] = city;
        return View(weather);

    }
    public async Task<IActionResult> BusinessData()
    {
        var data = await _requestService.GetPrices();
        return View(data);
    }

    [Authorize]
    public async Task<IActionResult> EditorsChoiced()
    {
        // Get the authenticated user's ID
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Retrieve the subscription for the authenticated user
        var subscription = await _subscriptionService.GetUserSubscriptionAsync(userId);

        // Check if the subscription exists and its SubscriptionType is not null
        if (subscription == null && !(User.IsInRole("Admin") || User.IsInRole("Editor") || User.IsInRole("Writer")))
        {
            TempData["Error"] = "You don't have a subscription. Please subscribe to access premium content.";
            return RedirectToAction("Subscribe", "Subscription"); // Redirect to the Subscription page
        }
        // Ensure that SubscriptionType is loaded and not null
        if (subscription?.SubscriptionType == null)
        {
            TempData["Error"] = "Your subscription type is not valid. Please contact support.";
            return RedirectToAction("NoAccess", "Home"); // Redirect to No Access page or similar
        }
        // Pass the subscription type to the view
        ViewBag.SubscriptionType = subscription.SubscriptionType.TypeName;

        // Fetch the Editors' Choice articles
        var articles = _articleService.EditorsChoice();

        // Return the view with the articles
        return View(articles);
    }



    public IActionResult NoAccess()
    {
        return View();
    }
}

