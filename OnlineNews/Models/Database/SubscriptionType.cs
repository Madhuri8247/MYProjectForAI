using System.ComponentModel.DataAnnotations;

namespace OnlineNews.Models.Database
{
    public class SubscriptionType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Subscription Type is required.")]
        public string TypeName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}