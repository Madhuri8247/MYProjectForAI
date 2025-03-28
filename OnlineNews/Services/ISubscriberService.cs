using OnlineNews.Models;
using OnlineNews.Models.Database;

namespace OnlineNews.Service
{
    public interface ISubscriptionService
    {
        Task<Subscription> PaymentConfirmation(string userId);
        Task<Subscription> GetUserSubscriptionAsync(string userId);
      
        Task<string> GetSubscriptionTypeByNameAsync(string subscriptionType);
        Task AddSubscriptionAsync(Subscription subscription, string subscriptionType);
        Task <bool> ChangeSubscriptionTypeAsync(string userId, string subscriptionType);

        List<SubscriptionItem> GetSubCount();
    }
}