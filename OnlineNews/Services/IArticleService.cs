
using Microsoft.AspNetCore.Mvc;
using OnlineNews.Models.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineNews.Interfaces
{
    public interface IArticleService
    {
        public void AddArticle(Article article, string authorId);
        public Article GetDetails(int id);
        public List<Article> GetAllArticles();
        public List<string> GetAllCategories();
        public List<Article> Mostpopular();
        public List<Article> EditorsChoice();
        public List<Article> OneLatestNews();
        public List<Article> GetAllArticlesByItsCategory(int categoryId);
        public List<Article> SomeLatestNews();
        public Article GetArticleById(int id);
        void UpdateArticleApproval(int id, bool isApproved);
        public bool HasConsented(IHttpContextAccessor httpContextAccessor);
        public void AcceptCookies(IHttpContextAccessor httpContextAccessor);
        public void DeclineCookies(IHttpContextAccessor httpContextAccessor);
        public void UpdateArticle(int id, Article updatedArticle);
        public void UpdateArticleViews(int id, int views);
        public UserInteractionWithArticle GetUserArticleInteraction(string userId, int articleId);
        public void AddArticleInteraction(string userId, int articleId, bool liked, bool disliked);
        public void UpdateArticleInteraction(string userId, int articleId, bool liked, bool disliked);
        void UpdateArticleApproval(int id, string status);
        public List<Article> GetAllArticles(bool isEditor);
        public List<Article> GetApprovedArticles();
    }
}
