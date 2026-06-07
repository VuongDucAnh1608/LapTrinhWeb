using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Website_QuanLyKhoHangThucPham.Data;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Controllers
{
    public class StoreController : Controller
    {
        private readonly AppDbContext _db;
        private const string CART_KEY = "CART_V1";

        public StoreController(AppDbContext db) => _db = db;

        // ── Trang cửa hàng (public, không cần đăng nhập) ─────────────────
        public async Task<IActionResult> Index(string? search, int? categoryId, int page = 1)
        {
            int pageSize = 12;
            var query = _db.Products
                .Include(p => p.Category).Include(p => p.Batches)
                .Where(p => p.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search));
            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId);

            var total    = await query.CountAsync();
            var products = await query.OrderBy(p => p.CategoryId).ThenBy(p => p.Name)
                                      .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.Categories  = new SelectList(await _db.Categories.Where(c => c.IsActive).ToListAsync(), "Id", "Name");
            ViewBag.Search      = search;
            ViewBag.CategoryId  = categoryId;
            ViewBag.CartCount   = GetCart().TotalItems;

            return View(new PagedResult<Website_QuanLyKhoHangThucPham.Models.Product>
            { Items = products, TotalCount = total, Page = page, PageSize = pageSize });
        }

        // ── Autocomplete: trả về kết quả khác nhau tùy trạng thái đăng nhập ──
        // Chưa đăng nhập: chỉ trả tên + giá (không lộ tồn kho, nhà cung cấp)
        // Đã đăng nhập: trả thêm tồn kho, SKU
        [HttpGet]
        public async Task<IActionResult> Autocomplete(string term)
        {
            if (string.IsNullOrWhiteSpace(term) || term.Length < 1)
                return Json(new object[0]);

            var normalized = term.ToLower();
            var query = _db.Products
                .Include(p => p.Category)
                .Include(p => p.Batches)
                .Where(p => p.IsActive && p.Name.ToLower().Contains(normalized))
                .Take(8);

            var isLoggedIn = User.Identity?.IsAuthenticated == true;

            if (isLoggedIn)
            {
                // Staff thấy thêm thông tin tồn kho
                var data = await query.Select(p => new
                {
                    id       = p.Id,
                    label    = p.Name,
                    category = p.Category!.Name,
                    price    = p.SellPrice,
                    unit     = p.Unit,
                    sku      = p.SKU,
                    stock    = p.Batches.Where(b => b.ExpiryDate > DateTime.UtcNow).Sum(b => b.Quantity)
                }).ToListAsync();
                return Json(data);
            }
            else
            {
                // Khách chỉ thấy tên + giá + danh mục — bảo vệ dữ liệu kinh doanh
                var data = await query.Select(p => new
                {
                    id       = p.Id,
                    label    = p.Name,
                    category = p.Category!.Name,
                    price    = p.SellPrice,
                    unit     = p.Unit
                }).ToListAsync();
                return Json(data);
            }
        }

        // ── Chi tiết sản phẩm ──────────────────────────────────────────────
        public async Task<IActionResult> ProductDetail(int id)
        {
            var product = await _db.Products
                .Include(p => p.Category).Include(p => p.Supplier).Include(p => p.Batches)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
            if (product == null) return NotFound();
            ViewBag.CartCount = GetCart().TotalItems;
            return View(product);
        }

        // ── Giỏ hàng ──────────────────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var product = await _db.Products.Include(p => p.Batches)
                .FirstOrDefaultAsync(p => p.Id == productId && p.IsActive);
            if (product == null) return Json(new { success = false, message = "San pham khong ton tai." });

            var stock = product.TotalStock;
            var cart  = GetCart();
            var existing = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            var currentQty = (existing?.Quantity ?? 0) + quantity;

            if (currentQty > stock)
                return Json(new { success = false, message = $"Ton kho chi con {stock} {product.Unit}." });

            if (existing != null) existing.Quantity += quantity;
            else cart.Items.Add(new CartItem
            {
                ProductId   = product.Id,
                ProductName = product.Name,
                Unit        = product.Unit,
                SellPrice   = product.SellPrice,
                Quantity    = quantity,
                ImageUrl    = product.ImageUrl
            });

            SaveCart(cart);
            return Json(new { success = true, totalItems = cart.TotalItems, message = $"Da them \"{product.Name}\" vao gio hang!" });
        }

        [HttpPost]
        public IActionResult UpdateCart(int productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null) { if (quantity <= 0) cart.Items.Remove(item); else item.Quantity = quantity; }
            SaveCart(cart);
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            cart.Items.RemoveAll(i => i.ProductId == productId);
            SaveCart(cart);
            return RedirectToAction(nameof(Cart));
        }

        public IActionResult Cart() => View(GetCart());

        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Items.Any()) { TempData["Error"] = "Gio hang trong!"; return RedirectToAction(nameof(Cart)); }
            return View(new CheckoutViewModel { Items = cart.Items });
        }

        // ── Helpers ───────────────────────────────────────────────────────
        private CartViewModel GetCart()
        {
            var json = HttpContext.Session.GetString(CART_KEY);
            return string.IsNullOrEmpty(json) ? new CartViewModel()
                : JsonSerializer.Deserialize<CartViewModel>(json) ?? new CartViewModel();
        }

        public CartViewModel GetCartPublic() => GetCart();

        private void SaveCart(CartViewModel cart)
            => HttpContext.Session.SetString(CART_KEY, JsonSerializer.Serialize(cart));
    }
}
