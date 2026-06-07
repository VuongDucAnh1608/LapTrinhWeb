using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Website_QuanLyKhoHangThucPham.Models;
using Website_QuanLyKhoHangThucPham.Services;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Controllers
{
    [Authorize]
    public class ExportRequestController : Controller
    {
        private readonly IExportService _exportService;

        public ExportRequestController(IExportService exportService)
        {
            _exportService = exportService;
        }

        [Authorize(Policy = "AdminOrWarehouse")]
        public async Task<IActionResult> Index(ExportStatus? status, int page = 1)
        {
            var result = await _exportService.GetPagedAsync(status, page);
            ViewBag.StatusFilter = status;
            return View(result);
        }

        [Authorize(Policy = "SalesOrAbove")]
        public async Task<IActionResult> MyRequests(int page = 1)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _exportService.GetByUserAsync(userId, page);
            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var request = await _exportService.GetByIdAsync(id);
            if (request == null) return NotFound();
            return View(request);
        }

        [Authorize(Policy = "SalesOrAbove")]
        public async Task<IActionResult> Create()
        {
            var products = await _exportService.GetAvailableProductsAsync();
            ViewBag.Products = new SelectList(products, "Id", "Name");
            return View(new ExportRequestViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = "SalesOrAbove")]
        public async Task<IActionResult> Create(ExportRequestViewModel model)
        {
            if (model.Details == null || !model.Details.Any())
                ModelState.AddModelError("", "Yeu cau xuat hang phai co it nhat 1 san pham.");

            if (!ModelState.IsValid)
            {
                var products = await _exportService.GetAvailableProductsAsync();
                ViewBag.Products = new SelectList(products, "Id", "Name");
                return View(model);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _exportService.CreateAsync(model, userId);
            TempData["Success"] = "Gui yeu cau xuat hang thanh cong!";
            return RedirectToAction(nameof(MyRequests));
        }

        [Authorize(Policy = "AdminOrWarehouse")]
        public async Task<IActionResult> Process(int id)
        {
            var request = await _exportService.GetByIdAsync(id);
            if (request == null || request.Status != ExportStatus.Pending)
                return RedirectToAction(nameof(Index));

            var model = new ProcessExportViewModel
            {
                RequestId = id,
                Items = request.Details.Select(d => new ActualQuantityViewModel
                {
                    DetailId = d.Id,
                    ProductId = d.ProductId,
                    ProductName = d.Product?.Name,
                    RequestedQuantity = d.RequestedQuantity,
                    AvailableStock = d.Product?.TotalStock ?? 0,
                    ActualQuantity = d.RequestedQuantity
                }).ToList()
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOrWarehouse")]
        public async Task<IActionResult> Process(ProcessExportViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _exportService.ApproveAsync(model.RequestId, model, userId);
            TempData["Success"] = "Da duyet va xuat kho thanh cong!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOrWarehouse")]
        public async Task<IActionResult> Reject(int id, string reason)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _exportService.RejectAsync(id, reason, userId);
            TempData["Success"] = "Da tu choi yeu cau xuat hang.";
            return RedirectToAction(nameof(Index));
        }
    }
}
