using Microsoft.AspNetCore.Identity;
using OnlineNews.Data;
using OnlineNews.Models.Database;

namespace OnlineNews.Models.Helper
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            if (!context.SubscriptionTypes.Any()) 
            {
                context.SubscriptionTypes.AddRange(
                    new SubscriptionType { TypeName = "Basic", Price = 10.00m, Description = "Basic subscription plan." },
                    new SubscriptionType { TypeName = "Premium", Price = 20.00m, Description = "Premium subscription plan with additional features." }
                );
                context.SaveChanges();
            }
        }
    }
}
