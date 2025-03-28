using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineNews.Data;
using OnlineNews.Interfaces;
using OnlineNews.Models.Database;
using OnlineNews.Services;

namespace OnlineNews.Controllers
{
    [ApiController]
    [Route("Editor")]
    public class EditorController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IArticleService _articleService;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<EditorController> _logger;
        

        public EditorController(UserManager<User> userManager, IArticleService articleService, ApplicationDbContext db, ILogger<EditorController> logger)
        {
            _userManager = userManager;
            _articleService = articleService;
            _db = db;
            _logger = logger;
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticles(string status = "Approved")
        {
            var user = await _userManager.GetUserAsync(User);
            bool isEditor = await _userManager.IsInRoleAsync(user, "Editor");

            ViewData["isEditor"] = isEditor; 

            List<Article> articles;

            // Filter articles based on the selected status
            if (status == "All")
            {
                articles = _articleService.GetAllArticles(isEditor); 
            }
            else
            {
                articles = _articleService.GetAllArticles(isEditor).Where(a => a.ApprovalStatus == status).ToList();
            }

            return View(articles);
        }

        [HttpPost("approve/{id}")]
        public IActionResult ApproveArticle(int id)
        {
            var article = _articleService.GetArticleById(id);
            if (article == null)
            {
                return NotFound("Article not found");
            }
            _articleService.UpdateArticleApproval(id, "Approved");
            return RedirectToAction("GetAllArticles");
        }

        [HttpPost("reject/{id}")]
        public IActionResult RejectArticle(int id)
        {
            var article = _articleService.GetArticleById(id);
            if (article == null)
            {
                return NotFound("Article not found");
            }
            _articleService.UpdateArticleApproval(id, "Rejected");
            return RedirectToAction("GetAllArticles");
        }

        public IActionResult UpdateArticleStatus(int id, string status)
        {
            var article = _articleService.GetArticleById(id);
            if(article == null)
            {
                return NotFound("Article not found");
            }
            if (new[] {"Approved", "Rejected", "Pending"}.Contains(status))
            {
                _articleService.UpdateArticleApproval(id, status);
                return RedirectToAction("GetAllArticles");
            }
            return BadRequest("Invalid status");
        }


        //public enum ArticleStatus
        //{
        //    Waiting = 1,
        //    Pending = 2,
        //    Rejected = 3,
        //    Approved = 4
        //}
        
    }
}
