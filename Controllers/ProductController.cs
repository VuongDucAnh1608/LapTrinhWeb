using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Website_QuanLyKhoHangThucPham.Services;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IAuditService _auditService;

        public ProductController(IProductService productService, IAuditService auditService)
        {
            _productService = productService;
            _auditService = auditService;
        }

        public async Task<IActionResult> Index(string? search, int? categoryId, int page = 1)
        {
            var result = await _productService.GetPagedAsync(search, categoryId, page, 10);
            ViewBag.Search = search;
            ViewBag.CategoryId = categoryId;
            ViewBag.Categories = new SelectList(await _productService.GetCategoriesAsync(), "Id", "Name");
            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        [Authorize(Policy = "AdminOrWarehouse")]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View(new ProductViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOrWarehouse")]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (!ModelState.IsValid) { await PopulateDropdowns(); return View(model); }
            var product = await _productService.CreateAsync(model);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _auditService.LogAsync(userId, "CREATE", "Product", product.Id.ToString(),
                null, $"Them san pham: {product.Name}");
            TempData["Success"] = $"Them san pham '{product.Name}' thanh cong!";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "AdminOrWarehouse")]
        public async Task<IActionResult> Edit(int id)
        {
            var p = await _productService.GetByIdAsync(id);
            if (p == null) return NotFound();
            await PopulateDropdowns();
            return View(new ProductViewModel
            {
                Id = p.Id, Name = p.Name, SKU = p.SKU, CategoryId = p.CategoryId,
                SupplierId = p.SupplierId, Unit = p.Unit, CostPrice = p.CostPrice,
                SellPrice = p.SellPrice, MinStockLevel = p.MinStockLevel,
                Description = p.Description, ExistingImageUrl = p.ImageUrl
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOrWarehouse")]
        public async Task<IActionResult> Edit(int id, ProductViewModel model)
        {
            if (!ModelState.IsValid) { await PopulateDropdowns(); return View(model); }
            var updated = await _productService.UpdateAsync(id, model);
            if (updated == null) return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _auditService.LogAsync(userId, "UPDATE", "Product", id.ToString());
            TempData["Success"] = "Cap nhat san pham thanh cong!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOrWarehouse")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            TempData["Success"] = "Da xoa san pham thanh cong.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Autocomplete(string term)
        {
            var suggestions = await _productService.SearchSuggestionsAsync(term ?? "");
            return Json(suggestions);
        }

        private async Task PopulateDropdowns()
        {
            ViewBag.Categories = new SelectList(await _productService.GetCategoriesAsync(), "Id", "Name");
            ViewBag.Suppliers = new SelectList(await _productService.GetSuppliersAsync(), "Id", "Name");
        }
    }
}
