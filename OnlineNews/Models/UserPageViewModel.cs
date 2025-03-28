using OnlineNews.Models.Database;

namespace OnlineNews.Models.ViewModels
{
    public class UserPageViewModel
    {
        public User User { get; set; }
        public Subscription Subscription { get; set; }

        // Read-only property for calculating remaining days
        public int RemainingDays
        {
            get
            {
                if (Subscription != null && Subscription.ExpiredAt > DateTime.UtcNow)
                {
                    return (Subscription.ExpiredAt - DateTime.UtcNow).Days;
                }
                return 0; // If no subscription or expired
            }
        }
    }
}
