using BlogData.Entities;
using BlogData.Repositories;
using System.Threading.Tasks;

namespace BlogData.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<Article> ArticleRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        IRepository<Tag> TagRepository { get; }
        IRepository<ArticleTag> ArticleTagRepository { get; }
        IRepository<Comment> CommentRepository { get; }
        IRepository<Like> LikeRepository { get; }
        IRepository<Notification> NotificationRepository { get; }
        Task SaveChangesAsync();
    }
}
