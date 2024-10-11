using BlogData.Entities;
using BlogData.Repositories;
using System.Threading.Tasks;

namespace BlogData.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRepository<Article> _articleRepository;
        private IRepository<Category> _categoryRepository;
        private IRepository<Tag> _tagRepository;
        private IRepository<ArticleTag> _articleTagRepository;
        private IRepository<Comment> _commentRepository;
        private IRepository<Like> _likeRepository;
        private IRepository<Notification> _notificationRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IRepository<Article> ArticleRepository
            => _articleRepository ??= new Repository<Article>(_context);

        public IRepository<Category> CategoryRepository
            => _categoryRepository ??= new Repository<Category>(_context);

        public IRepository<Tag> TagRepository
            => _tagRepository ??= new Repository<Tag>(_context);

        public IRepository<ArticleTag> ArticleTagRepository
            => _articleTagRepository ??= new Repository<ArticleTag>(_context);

        public IRepository<Comment> CommentRepository
            => _commentRepository ??= new Repository<Comment>(_context);

        public IRepository<Like> LikeRepository
            => _likeRepository ??= new Repository<Like>(_context);

        public IRepository<Notification> NotificationRepository
            => _notificationRepository ??= new Repository<Notification>(_context);

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
