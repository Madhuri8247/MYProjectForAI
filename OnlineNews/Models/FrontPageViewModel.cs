using OnlineNews.Models.Database;

namespace OnlineNews.Models
{
    public class FrontPageViewModel
    {

        public List<Article> Mostpopular { get; set; } = new List<Article>();
        public List<Article> OneLatestNews { get; set; } = new List<Article>();
        public List<Article> SomeLatestNews{ get; set; } = new List<Article>();
        public List<Article> EditorsChoiceArticles { get; set; } = new List<Article>();

    }
}
