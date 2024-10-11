using BlogData.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BlogData.Entities
{

    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CommentDate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedDate { get; set; } // Bu özelliği kontrol edin
    }
}