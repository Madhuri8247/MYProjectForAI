using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using OnlineNews.Data;
using OnlineNews.Models.Database;
using OnlineNews.Service;
using System.Data;
using System.Security.Claims;


namespace OnlineNews.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly IAdminService _adminService;
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ISubscriptionService _subscriptionService;

        public AdminController(UserManager<User> userManager, IAdminService adminService, 
            RoleManager<IdentityRole> roleManager, IUserService userService, ISubscriptionService subscriptionService)
        {
            _userManager = userManager;
            _adminService = adminService;
            _roleManager = roleManager;
            _userService = userService;
            _subscriptionService = subscriptionService;
        }
        public async Task<IActionResult> ListUsers()
        { 
            var users = _userService.GetUsersWithRoles();
            ViewBag.Roles = _roleManager.Roles.ToList();
            ViewBag.SubCount = _subscriptionService.GetSubCount();
            return View(users); 
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



        public async Task<IActionResult> AddRoleToUser(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (!currentRoles.Contains(role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
            return RedirectToAction(nameof(ListUsers));
        }



        public async Task<IActionResult> RemoveRoleFromUser(string userId) 
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in currentRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }
            }
            return RedirectToAction(nameof(ListUsers));
        }


        
        public async Task<IActionResult> ToggleUserStatus(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = !user.IsActive; // Toggle the active status
                await _userManager.UpdateAsync(user); // Save changes to the user
            }
            return RedirectToAction(nameof(ListUsers)); // Redirect back to the user list
        }



        //[Authorize(Roles = "Admin")]
        //private readonly IAdminService _adminService;
        //private readonly UserManager<User> _userManager;


        //public AdminController(IAdminService adminService, UserManager<User> userManager)
        //{ 
        //    _adminService = adminService;
        //    _userManager = userManager;
        //}

        //public async Task <IActionResult> Index()
        //{

        //    var roleResult = await _adminService.CreateRole();
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    var assignRoleResult = await _adminService.AddRoleToEmployee(userId);
        //    return View();
        //}

        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}


    }
}
