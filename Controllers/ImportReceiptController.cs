using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Website_QuanLyKhoHangThucPham.Services;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Controllers
{
    [Authorize(Policy = "AdminOrWarehouse")]
    public class ImportReceiptController : Controller
    {
        private readonly IImportService _importService;
        private readonly IProductService _productService;

        public ImportReceiptController(IImportService importService, IProductService productService)
        {
            _importService = importService;
            _productService = productService;
        }

        public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate, int page = 1)
        {
            var model = await _importService.GetPagedAsync(fromDate, toDate, page);
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var receipt = await _importService.GetByIdAsync(id);
            if (receipt == null) return NotFound();
            return View(receipt);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Suppliers = new SelectList(await _importService.GetSuppliersAsync(), "Id", "Name");
            return View(new ImportReceiptViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImportReceiptViewModel model)
        {
            if (model.Details == null || !model.Details.Any())
                ModelState.AddModelError("", "Phiếu nhập phải có ít nhất 1 sản phẩm.");

            if (!ModelState.IsValid)
            {
                ViewBag.Suppliers = new SelectList(await _importService.GetSuppliersAsync(), "Id", "Name");
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _importService.CreateAsync(model, userId);
            TempData["Success"] = "Lập phiếu nhập thành công!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirm(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _importService.ConfirmAsync(id, userId);
            return Json(new { success = result });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _importService.CancelAsync(id, userId);
            TempData[result ? "Success" : "Error"] = result ? "Đã hủy phiếu nhập." : "Không thể hủy phiếu này.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExpiringBatches()
        {
            var batches = await _importService.GetExpiringBatchesAsync(30);
            return View(batches);
        }
    }
}
