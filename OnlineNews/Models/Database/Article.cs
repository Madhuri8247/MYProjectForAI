using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static OnlineNews.Controllers.EditorController;

namespace OnlineNews.Models.Database
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Publishing Date of Article is required.")]
        [Display(Name = "Publishing Date")]
        public DateTime PublishedDate { get; set; }

        public string LinkText { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Headline of Article is required.")]
        [Display(Name = "Headline of Article")]
        [StringLength(100)]
        public string Headline { get; set; }

        [Required(ErrorMessage = "ContentSummary of Article is required.")]
        [Display(Name = "ContentSummary of Article")]
        public string ContentSummary { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content of Article is required.")]
        [Display(Name = "Content of Article")]
        public string Content { get; set; } = string.Empty;
        public bool IsArchieved { get; set; }
        public int Views { get; set; } = 0;
        public int Likes { get; set; } = 0;
        public string ImageLink { get; set; } = string.Empty;

        [ValidateNever]
        public Category? Category { get; set; } = new Category();

        [NotMapped]
        public string ChosenCategory { get; set; } = string.Empty;

        [NotMapped]
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public User? Author { get; set; }
        public bool EditorsChoice { get; set; }

        //public bool IsApproved { get; set; }
        public string ApprovalStatus { get; set; } = "Pending";

        //public ArticleStatus Status { get; set; } = ArticleStatus.Waiting;
    }
}
