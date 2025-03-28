using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace OnlineNews.Models
{
    public class OrderRow
    {
        public int Id { get; set; }

        [ForeignKey("Order")]
        [Required(ErrorMessage = "Order Id is required.")]
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        [ForeignKey("Product")]
        [Required(ErrorMessage = "Product Id is required.")]
        [Display(Name = "Product Id")]
        public int ProductId { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price of Product is required.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Product? product { get; set; }
    }
}
