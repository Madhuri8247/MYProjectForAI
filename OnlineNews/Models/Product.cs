using System.ComponentModel.DataAnnotations;

namespace OnlineNews.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required] 
        [StringLength(100)] 
        public string Name { get; set; }

        [Required] 
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [StringLength(500)] 
        public string Description { get; set; }

        [Required] 
        public string Category { get; set; }

        public string ImageUrl { get; set; } 

        [Required]
        public bool IsAvailable { get; set; } 

        [Range(0, int.MaxValue)] 
        public int StockQuantity { get; set; }

        [DataType(DataType.Date)] 
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public virtual ICollection<OrderRow> OrderRows { get; set; } = new List<OrderRow>();

    }
}
