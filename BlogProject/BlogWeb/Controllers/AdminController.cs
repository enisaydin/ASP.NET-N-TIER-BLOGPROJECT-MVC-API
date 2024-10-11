using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BlogWeb.Models;
using System.Linq;
using System.Threading.Tasks;
using BlogData.Entities;
using BlogData;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Kullanıcıları listeleme
    public async Task<IActionResult> Users()
    {
        var users = await _userManager.Users.ToListAsync();
        return View(users);
    }

    // Yazarları listeleme
    public async Task<IActionResult> Writers()
    {
        var users = _userManager.Users.Where(u => _userManager.IsInRoleAsync(u, "Writer").Result);
        return View(await users.ToListAsync());
    }

    // Makaleleri listeleme
    public async Task<IActionResult> Articles()
    {
        var articles = await _context.Articles
            .Include(a => a.Category)  // Category'yi dahil et
            .Include(a => a.Author)    // Author'ı dahil et
            .Include(a => a.ArticleTags)  // ArticleTags'ı dahil et
            .ThenInclude(at => at.Tag)  // Her ArticleTag için Tag'i dahil et
            .ToListAsync();

        return View(articles);
    }

    // Yorumları listeleme
    public async Task<IActionResult> Comments()
    {
        var comments = await _context.Comments.Include(c => c.Article).Include(c => c.User).ToListAsync();
        return View(comments);
    }

    // Kullanıcıları düzenleme
    public async Task<IActionResult> EditUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(ApplicationUser model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = model.UserName;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Users));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    // Kullanıcıları silme
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    [HttpPost, ActionName("DeleteUser")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUserConfirmed(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Users));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(user);
    }

    // Makaleyi düzenleme
    public async Task<IActionResult> EditArticle(int id)
    {
        var article = await _context.Articles.Include(a => a.Category).FirstOrDefaultAsync(a => a.ArticleId == id);
        if (article == null)
        {
            return NotFound();
        }
        return View(article);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditArticle(Article model)
    {
        if (ModelState.IsValid)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Articles));
        }
        return View(model);
    }

    // Makaleyi silme
    public async Task<IActionResult> DeleteArticle(int id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article == null)
        {
            return NotFound();
        }
        return View(article);
    }

    [HttpPost, ActionName("DeleteArticle")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteArticleConfirmed(int id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article != null)
        {
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Articles));
    }

    // Yorumu silme
    public async Task<IActionResult> DeleteComment(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }
        return View(comment);
    }

    [HttpPost, ActionName("DeleteComment")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCommentConfirmed(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment != null)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Comments));
    }
}
