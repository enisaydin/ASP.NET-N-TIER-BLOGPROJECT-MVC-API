using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BlogData.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfileInfo { get; set; } = string.Empty;
        public ICollection<Article> Articles { get; set; } = new List<Article>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
