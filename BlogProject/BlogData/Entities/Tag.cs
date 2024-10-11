using BlogData.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BlogData.Entities
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }

        public ICollection<ArticleTag> ArticleTags { get; set; } = new List<ArticleTag>();  // Etiket makale ilişkisi
    }
}