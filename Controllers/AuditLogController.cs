using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_QuanLyKhoHangThucPham.Data;

namespace Website_QuanLyKhoHangThucPham.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AuditLogController : Controller
    {
        private readonly AppDbContext _db;

        public AuditLogController(AppDbContext db) { _db = db; }

        public async Task<IActionResult> Index(string? action, string? user, int page = 1)
        {
            var query = _db.AuditLogs.AsQueryable();
            if (!string.IsNullOrEmpty(action))
                query = query.Where(l => l.Action.Contains(action));
            if (!string.IsNullOrEmpty(user))
                query = query.Where(l => l.UserName.Contains(user));

            int pageSize = 20;
            var total = await query.CountAsync();
            var items = await query.OrderByDescending(l => l.Timestamp)
                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.TotalCount = total;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.ActionFilter = action;
            ViewBag.UserFilter = user;
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var log = await _db.AuditLogs.FindAsync(id);
            if (log == null) return NotFound();
            return View(log);
        }
    }
}
