using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using OnlineNews.Models.Database;
using System.Data;

namespace OnlineNews.Service
{
    public class AdminService: IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;

        }
        public async Task<string> AddAdminRoleToEmployee(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            var result = _userManager.AddToRoleAsync(user, "Admin").Result;

            if (result.Succeeded)
            {
                return "Success";
            }
            return "Failure";
        }

        public async Task<string> RemoveAdminRoleFromEmployee(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            var result = _userManager.RemoveFromRoleAsync(user, "Admin").Result;

            if (result.Succeeded)
            {
                return "Success";
            }
            return "Failure";
        }

        public async Task<string> CreateRole()
        {
            if (!await _roleManager.RoleExistsAsync("DeleteMe"))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole("DeleteMe"));

                if (result.Succeeded)
                {
                    return "Success";
                }

            }
            return "Failure";
        }


        public async Task<string> FindRole(User user)
        {
            string role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            return role;
        }

        public async Task<string> FindUser(User user)
        {
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            return role;
        }

        public async Task<string> AddRoleToUser(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (!currentRoles.Contains(role))
                {
                    var result = await _userManager.AddToRoleAsync(user, role);
                    if (result.Succeeded)
                    {
                        return "Role Added";
                    }
                }
            }
            return "Failed to Add Role";
        }

        public async Task<string> RemoveRoleFromUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in currentRoles)
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, role);
                    if (!result.Succeeded)
                    {
                        return "Failed to Remove Role";
                    }
                }
                return "Role Removed";
            }
            return "User Not Found";
        }

    }
    
}
