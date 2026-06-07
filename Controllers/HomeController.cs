using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Website_QuanLyKhoHangThucPham.Data;
using Website_QuanLyKhoHangThucPham.Models;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db) { _db = db; }

        public async Task<IActionResult> Index()
        {
            var now = DateTime.UtcNow;
            var today = now.Date;

            ViewBag.TotalProducts   = await _db.Products.CountAsync(p => p.IsActive);
            ViewBag.TotalCategories = await _db.Categories.CountAsync(c => c.IsActive);
            ViewBag.TotalSuppliers  = await _db.Suppliers.CountAsync(s => s.IsActive);
            ViewBag.PendingExports  = await _db.ExportRequests.CountAsync(r => r.Status == ExportStatus.Pending);
            ViewBag.TotalOrders     = await _db.StoreOrders.CountAsync(o => o.CreatedAt >= today);
            ViewBag.ExpiringBatches = await _db.InventoryBatches
                .CountAsync(b => b.ExpiryDate <= now.AddDays(30) && b.ExpiryDate > now && b.Quantity > 0);

            // Load sản phẩm vào memory trước, FEFO TotalStock là computed property
            var allProducts = await _db.Products
                .Include(p => p.Batches)
                .Where(p => p.IsActive)
                .ToListAsync();

            ViewBag.LowStockList = allProducts
                .Where(p => p.TotalStock <= p.MinStockLevel && p.TotalStock > 0)
                .OrderBy(p => p.TotalStock)
                .Take(5)
                .ToList();

            ViewBag.RecentImports = await _db.ImportReceipts
                .Include(r => r.Supplier)
                .OrderByDescending(r => r.ImportDate)
                .Take(5).ToListAsync();

            ViewBag.RecentStoreOrders = await _db.StoreOrders
                .OrderByDescending(o => o.CreatedAt)
                .Take(5).ToListAsync();

            ViewBag.CategorySales = await _db.InventoryBatches
                .Include(b => b.Product).ThenInclude(p => p!.Category)
                .Where(b => b.Quantity > 0 && b.ExpiryDate > now)
                .GroupBy(b => b.Product!.Category!.Name)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    TotalValue   = g.Sum(b => b.Quantity * b.Product!.SellPrice)
                })
                .OrderByDescending(x => x.TotalValue)
                .Take(8).ToListAsync();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
