using System.ComponentModel.DataAnnotations;

namespace OnlineNews.Models.Database
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        [Display (Name="Category Name")]
        public string Name { get; set; }

        public virtual ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
