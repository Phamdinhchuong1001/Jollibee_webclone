using JollibeeClone.Data;
using JollibeeClone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JollibeeClone.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(AppDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Danh sách tin tức hiển thị khi load trang
        public List<dynamic> NewsList { get; set; } = new List<dynamic>();
        public string ConnectionTest { get; set; } = "";

        // Handler mặc định khi vào trang
        public async Task OnGetAsync()
        {
            ConnectionTest = "RAZOR_PAGE_ACTIVE";
            try
            {
                var newsData = await _context.News
                    .Include(n => n.Author)
                    .Where(n => n.IsPublished && n.NewsType == "News")
                    .OrderByDescending(n => n.PublishedDate)
                    .Select(n => new {
                        n.NewsID,
                        n.Title,
                        n.ShortDescription,
                        n.Content,
                        n.ImageUrl,
                        n.PublishedDate,
                        AuthorName = n.Author != null ? n.Author.FullName : "Admin"
                    })
                    .ToListAsync();

                NewsList = newsData.Cast<dynamic>().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải danh sách tin tức tại Razor Page");
            }
        }

        public async Task<IActionResult> OnGetNewsDetailsAsync(int id)
        {
            try
            {
                var news = await _context.News
                    .Include(n => n.Author)
                    .Where(n => n.NewsID == id && n.IsPublished && n.NewsType == "News")
                    .Select(n => new {
                        n.NewsID,
                        n.Title,
                        n.ShortDescription,
                        n.Content,
                        n.ImageUrl,
                        n.PublishedDate,
                        AuthorName = n.Author != null ? n.Author.FullName : "Admin"
                    })
                    .FirstOrDefaultAsync();

                if (news == null)
                {
                    return new JsonResult(new { success = false, message = "Tin tức không tồn tại." });
                }

                return new JsonResult(new { success = true, data = news });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi lấy chi tiết tin tức tại Page Handler");
                return new JsonResult(new { success = false, message = "Có lỗi xảy ra." });
            }
        }
    }
}