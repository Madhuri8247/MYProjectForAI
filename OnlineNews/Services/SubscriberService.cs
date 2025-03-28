using Microsoft.EntityFrameworkCore;
using OnlineNews.Data;
using OnlineNews.Models;
using OnlineNews.Models.Database;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineNews.Service
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ApplicationDbContext _db;

        public SubscriptionService(ApplicationDbContext db)
        {
            _db = db;
        }

        // Get the most recent subscription of the user
        public async Task<Subscription> PaymentConfirmation(string userId)
        {
            var subscription = await _db.Subscriptions
                .Include(s => s.SubscriptionType)
                .Where(s => s.Subscriber.Id == userId)
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync();
            // Optionally handle the case where no subscription is found
            if (subscription == null)
            {
                // Handle logic if no subscription is found (could throw an exception, return null, etc.)
                return null;
            }

            subscription.PaymentComplete = true;
            _db.Update(subscription);
            _db.SaveChanges();

            return subscription;
        }

        public async Task<Subscription> GetUserSubscriptionAsync(string userId)
        {
            var subscription = await _db.Subscriptions
                .Include(s => s.SubscriptionType)
                .Where(s => s.Subscriber.Id == userId)
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync();

            // Optionally handle the case where no subscription is found
            if (subscription == null)
            {
                // Handle logic if no subscription is found (could throw an exception, return null, etc.)
                return null;
            }

            return subscription;
        }


        // Change the subscription type of the user
        public async Task<bool> ChangeSubscriptionTypeAsync(string userId, string subscriptionType)
        {
            // Ensure the subscription type is valid
            var subscriptionTypeObject = await _db.SubscriptionTypes
                .Where(st => st.TypeName.ToLower() == subscriptionType.ToLower())
                .FirstOrDefaultAsync();

            if (subscriptionTypeObject == null)
            {
                return false; // Return false if the subscription type doesn't exist
            }

            // Get the latest subscription for the user
            var subscription = await _db.Subscriptions
                .Where(s => s.Subscriber.Id == userId)
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync();

            if (subscription != null)
            {
                // Change subscription type if subscription exists
                subscription.SubscriptionType = subscriptionTypeObject;
                _db.Subscriptions.Update(subscription);
                await _db.SaveChangesAsync();
                return true;
            }

            return false; // Return false if no subscription is found
        }

        // Get subscription type name by subscription type string
        public async Task<string> GetSubscriptionTypeByNameAsync(string subscriptionType)
        {
            var type = await _db.SubscriptionTypes
                .Where(st => st.TypeName.ToLower() == subscriptionType.ToLower())
                .Select(st => st.TypeName)
                .FirstOrDefaultAsync();

            return type; // Returns null if not found
        }

        // Add a new subscription for the user
        public async Task AddSubscriptionAsync(Subscription subscription, string subscriptionType)
        {
            // Check if subscription already exists to prevent duplicates
            //var existingSubscription = await _db.Subscriptions
            //    .Where(s => s.Subscriber.Id == subscription.Subscriber.Id)
            //    .FirstOrDefaultAsync();

            //if (existingSubscription != null)
            //{
            //    // Handle case if subscription already exists (you can throw an exception or return a result indicating so)
            //    return;
            //}

            // Add the new subscription
            var subType = _db.SubscriptionTypes.FirstOrDefault(st => st.TypeName == subscriptionType);
            subscription.SubscriptionType = subType;
            _db.Subscriptions.Add(subscription);
            await _db.SaveChangesAsync();
        }


        public List<SubscriptionItem> GetSubCount()
        {
            //var subscriptionCount = new { BasicCount = _db.Subscriptions.Count(s => s.SubscriptionType == "Basic"), 
            //    PremiumCount = _db.Subscriptions.Count(s => s.SubscriptionType == "Premium") };

            var result = _db.Subscriptions
            .GroupBy(s => s.SubscriptionType)
            .Select(g => new SubscriptionItem()
            {
                SubscriptionsCount = g.Count(),
                SubscriptionType = g.Key
            }).ToList();
            return result;
        }

    }
}
