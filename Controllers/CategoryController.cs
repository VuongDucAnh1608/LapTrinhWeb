using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Website_QuanLyKhoHangThucPham.Services;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IAuditService _auditService;

        public CategoryController(ICategoryService categoryService, IAuditService auditService)
        {
            _categoryService = categoryService;
            _auditService = auditService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync(activeOnly: false);
            return View(categories);
        }

        public IActionResult Create()
        {
            return View(new CategoryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var category = await _categoryService.CreateAsync(model);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _auditService.LogAsync(userId, "CREATE", "Category", category.Id.ToString(),
                HttpContext.Connection.RemoteIpAddress?.ToString(), $"Thêm danh mục: {category.Name}");

            TempData["Success"] = $"Thêm danh mục '{category.Name}' thành công!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();

            return View(new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var updated = await _categoryService.UpdateAsync(id, model);
            if (updated == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _auditService.LogAsync(userId, "UPDATE", "Category", id.ToString(),
                null, $"Cập nhật danh mục: {updated.Name}");

            TempData["Success"] = $"Cập nhật danh mục '{updated.Name}' thành công!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            if (!result)
                TempData["Error"] = "Không thể xóa danh mục này vì đang có sản phẩm liên kết.";
            else
            {
                TempData["Success"] = "Đã xóa danh mục thành công.";
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _auditService.LogAsync(userId, "DELETE", "Category", id.ToString());
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
