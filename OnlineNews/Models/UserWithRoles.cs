using OnlineNews.Models.Database;

namespace OnlineNews.Models
{
    public class UserWithRolesVM
    {
        public User User { get; set; }
        public List<string> Roles { get; set; }
    }
}
