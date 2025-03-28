using OnlineNews.Models.Database;

namespace OnlineNews.Models
{
    public class CategoryViewModel
    {
        public List<Article> Articles { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string CategoryFilter { get; set; } // For category filtering

    }
}
