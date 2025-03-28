using OnlineNews.Models.Database;

namespace OnlineNews.Service
{
    public interface IAdminService
    {

        Task<string> AddAdminRoleToEmployee(string userId);

        Task<string> RemoveAdminRoleFromEmployee(string userId);

        Task<string> AddRoleToUser(string userId, string role);
        Task<string> RemoveRoleFromUser(string userId);

        Task<string> CreateRole();

        Task<string> FindRole(User user );

        Task<string> FindUser(User user);


    }
}
