using System.Collections.Generic;

namespace BlogData.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
}
