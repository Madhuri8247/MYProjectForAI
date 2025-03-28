using System.ComponentModel.DataAnnotations;

namespace OnlineNews.Models.Database
{
    public class UserInteractionWithArticle
    {
        [Key]
        public int Id { get; set; }         // Primary key for the interaction record

        [Required]
        public string UserId { get; set; }  // Foreign key to User (IdentityUser)

        [Required]
        public int ArticleId { get; set; }  // Foreign key to Article

        public bool Liked { get; set; }     // Whether the user liked the article
        public bool Disliked { get; set; }  // Whether the user disliked the article

        // Navigation properties for relationships
        public virtual User User { get; set; }     // User who interacted
        public virtual Article Article { get; set; } // Article that was interacted with
    }
}

