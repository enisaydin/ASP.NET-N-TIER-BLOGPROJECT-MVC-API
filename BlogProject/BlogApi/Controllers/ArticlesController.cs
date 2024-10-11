using BlogData;
using BlogData.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleTagsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ArticleTagsController> _logger;

        public ArticleTagsController(ApplicationDbContext context, ILogger<ArticleTagsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST: api/ArticleTags
        [HttpPost]
        public async Task<ActionResult<ArticleTag>> PostArticleTag(ArticleTag articleTag)
        {
            if (articleTag == null)
            {
                return BadRequest("ArticleTag is null.");
            }

            // Ensure that the Article and Tag exist
            var articleExists = await _context.Articles.AnyAsync(a => a.ArticleId == articleTag.ArticleId);
            var tagExists = await _context.Tags.AnyAsync(t => t.TagId == articleTag.TagId);

            if (!articleExists)
            {
                return BadRequest("Article does not exist.");
            }

            if (!tagExists)
            {
                return BadRequest("Tag does not exist.");
            }

            _context.ArticleTags.Add(articleTag);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetArticleTags), new { id = articleTag.ArticleId }, articleTag);
        }

        // GET: api/ArticleTags
        [HttpGet]
        public async Task<IActionResult> GetArticleTags()
        {
            try
            {
                var articleTags = await _context.ArticleTags
                    .Include(at => at.Article)
                    .Include(at => at.Tag)
                    .ToListAsync();
                return Ok(articleTags);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching article tags.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
