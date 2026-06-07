using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Website_QuanLyKhoHangThucPham.Services;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;
        private readonly IAuditService _auditService;

        public SupplierController(ISupplierService supplierService, IAuditService auditService)
        {
            _supplierService = supplierService;
            _auditService = auditService;
        }

        public async Task<IActionResult> Index()
        {
            var suppliers = await _supplierService.GetAllAsync(activeOnly: false);
            return View(suppliers);
        }

        public IActionResult Create() => View(new SupplierViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupplierViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var supplier = await _supplierService.CreateAsync(model);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _auditService.LogAsync(userId, "CREATE", "Supplier", supplier.Id.ToString(),
                null, $"Them nha cung cap: {supplier.Name}");
            TempData["Success"] = $"Them '{supplier.Name}' thanh cong!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var s = await _supplierService.GetByIdAsync(id);
            if (s == null) return NotFound();
            return View(new SupplierViewModel
            {
                Id = s.Id, Name = s.Name, Code = s.Code, Address = s.Address,
                Phone = s.Phone, Email = s.Email, ContactPerson = s.ContactPerson, IsActive = s.IsActive
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SupplierViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var updated = await _supplierService.UpdateAsync(id, model);
            if (updated == null) return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _auditService.LogAsync(userId, "UPDATE", "Supplier", id.ToString());
            TempData["Success"] = "Cap nhat thanh cong!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _supplierService.DeleteAsync(id);
            TempData[result ? "Success" : "Error"] = result
                ? "Da xoa nha cung cap thanh cong."
                : "Khong the xoa vi dang co san pham lien ket.";
            return RedirectToAction(nameof(Index));
        }
    }
}
