using System;
using System.Collections.Generic;

namespace BlogData.Entities
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }

        // Navigation Properties
        public ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();
        public Category Category { get; set; } // Bu ilişkiyi doğru yapılandırdığınızdan emin olun
        public ApplicationUser? Author { get; set; } // Bu ilişkiyi doğru yapılandırdığınızdan emin olun
    }
}
