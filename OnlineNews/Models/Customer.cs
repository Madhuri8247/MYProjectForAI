using System.ComponentModel.DataAnnotations;

namespace OnlineNews.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(20, ErrorMessage = "First name can't be longer than 20 characters.")]
        [Display(Name = "First Name")]
        public string FirstNameBillingAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(20, ErrorMessage = "Last name can't be longer than 20 characters.")]
        [Display(Name = "Last Name")]
        public string LastNameBillingAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(20, ErrorMessage = "First name can't be longer than 20 characters.")]
        [Display(Name = "First Name")]
        public string FirstNameDeliveryAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(20, ErrorMessage = "Last name can't be longer than 20 characters.")]
        [Display(Name = "Last Name")]
        public string LastNameDeliveryAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Billing Address is required.")]
        [StringLength(50, ErrorMessage = "Billing Address can't be longer than 50 characters.")]
        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Billing Zip is required.")]
        [Display(Name = "Billing Zip")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Billing zip must be a 5-digit number.")]
        public string BillingZip { get; set; } = string.Empty;

        [Required(ErrorMessage = "Billing city is required.")]
        [StringLength(50, ErrorMessage = "Billing city can't be longer than 50 characters.")]
        [Display(Name = "Billing City")]
        public string BillingCity { get; set; } = string.Empty;

        [Required(ErrorMessage = "Delivery address is required.")]
        [StringLength(50, ErrorMessage = "Delivery address can't be longer than 50 characters.")]
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Delivery Zip is required.")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Delivery zip must be a 5-digit number.")]
        [Display(Name = "Delivery Zip")]
        public string DeliverZip { get; set; } = string.Empty;

        [Required(ErrorMessage = "Delivery City is required.")]
        [StringLength(50, ErrorMessage = "Delivery city can't be longer than 50 characters.")]
        [Display(Name = "Delivery City")]
        public string DeliverCity { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, ErrorMessage = "Phone number can't be longer than 15 digits.")]
        [Display(Name = "Phone Number")]
        public string PhoneNo { get; set; } = string.Empty;

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
