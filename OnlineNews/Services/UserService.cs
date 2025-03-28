using OnlineNews.Data;
using Microsoft.AspNetCore.Identity;
using OnlineNews.Models.Database;
using OnlineNews.Models;

namespace OnlineNews.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<string> AddEmployee()
        {
            User newEmployee = new User()
            {
                UserName = "Peter.Forsberg@gmail.com",
                Email = "Peter.Forsberg@gmail.com",
                FirstName = "Peter",
                LastName = "Forsberg",
                PhoneNumber = "1234567890",
            };
            var result = await _userManager.CreateAsync(newEmployee, "newPassword22");
            if (result.Succeeded)
            {
                return "Success";
            }
            return "Failure";
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        public List<User> GetUsersByIsActive()
        {
            var activeUsers = _userManager.Users.Where(x => x.IsActive).ToList();
            return activeUsers;
        }

        public List<UserWithRolesVM> GetUsersWithRoles()
        {
            return _userManager.Users
                .Select(u => new UserWithRolesVM
                {
                    User = u,
                    Roles = _userManager.GetRolesAsync(u).Result.ToList()
                })
                .ToList();
        }
    }
}