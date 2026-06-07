using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Website_QuanLyKhoHangThucPham.Data;
using Website_QuanLyKhoHangThucPham.Models;
using Website_QuanLyKhoHangThucPham.Services;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Controllers
{
    public class PaymentController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ISePayService _sepay;
        private const string CART_KEY = "CART_V1";

        public PaymentController(AppDbContext db, ISePayService sepay)
        {
            _db    = db;
            _sepay = sepay;
        }

        // POST /Payment/PlaceOrder — Tạo đơn và hiển thị QR thanh toán
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            var cart = GetCart();
            if (!cart.Items.Any()) { TempData["Error"] = "Gio hang trong!"; return RedirectToAction("Cart", "Store"); }

            model.Items = cart.Items;
            if (!ModelState.IsValid) return View("~/Views/Store/Checkout.cshtml", model);

            var orderCode = $"WH{DateTime.UtcNow:yyyyMMddHHmmss}";
            var order     = new StoreOrder
            {
                OrderCode       = orderCode,
                TotalAmount     = cart.Total,
                Status          = model.PaymentMethod == "COD" ? "COD" : "Pending",
                PaymentMethod   = model.PaymentMethod,
                CustomerName    = model.CustomerName,
                CustomerEmail   = model.CustomerEmail,
                CustomerPhone   = model.CustomerPhone,
                DeliveryAddress = model.DeliveryAddress,
                Note            = model.Note,
                CreatedAt       = DateTime.UtcNow,
                Items           = cart.Items.Select(i => new StoreOrderItem
                {
                    ProductId   = i.ProductId,
                    ProductName = i.ProductName,
                    UnitPrice   = i.SellPrice,
                    Quantity    = i.Quantity
                }).ToList()
            };

            _db.StoreOrders.Add(order);
            await _db.SaveChangesAsync();
            ClearCart();

            if (model.PaymentMethod == "COD")
                return RedirectToAction(nameof(Success), new { orderCode });

            // SePay — hiển thị QR code
            var qrUrl   = _sepay.GenerateQrUrl(orderCode, (long)cart.Total);
            var content = _sepay.GenerateTransferContent(orderCode);
            ViewBag.QrUrl      = qrUrl;
            ViewBag.OrderCode  = orderCode;
            ViewBag.Amount     = cart.Total;
            ViewBag.Content    = content;
            ViewBag.Order      = order;
            return View("QRPending");
        }

        // GET /Payment/CheckStatus — polling từ frontend mỗi 5 giây
        [HttpGet]
        public async Task<IActionResult> CheckStatus(string orderCode)
        {
            var order = await _db.StoreOrders.FirstOrDefaultAsync(o => o.OrderCode == orderCode);
            if (order == null) return Json(new { status = "NotFound" });
            return Json(new { status = order.Status, paidAt = order.PaidAt });
        }

        // POST /Payment/SePayWebhook — SePay gọi khi nhận được giao dịch khớp
        [HttpPost]
        public async Task<IActionResult> SePayWebhook()
        {
            using var reader  = new StreamReader(Request.Body);
            var rawBody       = await reader.ReadToEndAsync();
            var signature     = Request.Headers["X-Webhook-Token"].ToString();

            // Bỏ qua xác minh nếu chưa cấu hình secret (môi trường dev)
            if (!string.IsNullOrEmpty(Request.Headers["X-Webhook-Token"].ToString()))
            {
                if (!_sepay.VerifyWebhook(rawBody, signature))
                    return Unauthorized();
            }

            try
            {
                var data        = JsonSerializer.Deserialize<JsonElement>(rawBody);
                var content     = data.GetProperty("content").GetString() ?? "";
                var amount      = data.GetProperty("transferAmount").GetInt64();

                // SePay gửi nội dung chuyển khoản — tìm mã đơn hàng trong chuỗi
                var order = await _db.StoreOrders
                    .Where(o => o.Status == "Pending" && content.Contains(o.OrderCode))
                    .FirstOrDefaultAsync();

                if (order != null && amount >= (long)order.TotalAmount)
                {
                    order.Status         = "Paid";
                    order.TransactionRef = data.TryGetProperty("referenceCode", out var refCode)
                                            ? refCode.GetString() : null;
                    order.PaidAt         = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                }

                return Ok(new { success = true });
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET /Payment/Success
        public async Task<IActionResult> Success(string orderCode)
        {
            var order = await _db.StoreOrders.Include(o => o.Items)
                                             .ThenInclude(i => i.Product)
                                             .FirstOrDefaultAsync(o => o.OrderCode == orderCode);
            return View(order);
        }

        // GET /Payment/Failed
        public async Task<IActionResult> Failed(string orderCode)
        {
            var order = await _db.StoreOrders.FirstOrDefaultAsync(o => o.OrderCode == orderCode);
            return View(order);
        }

        // GET /Payment/History — Admin xem lịch sử đơn hàng
        [Microsoft.AspNetCore.Authorization.Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> History(string? status, int page = 1)
        {
            int pageSize = 20;
            var query    = _db.StoreOrders.Include(o => o.Items).AsQueryable();
            if (!string.IsNullOrEmpty(status)) query = query.Where(o => o.Status == status);
            var total = await query.CountAsync();
            var items = await query.OrderByDescending(o => o.CreatedAt)
                                   .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            ViewBag.TotalCount   = total;
            ViewBag.Page         = page;
            ViewBag.PageSize     = pageSize;
            ViewBag.StatusFilter = status;
            return View(items);
        }

        private CartViewModel GetCart()
        {
            var json = HttpContext.Session.GetString(CART_KEY);
            return string.IsNullOrEmpty(json) ? new CartViewModel()
                : JsonSerializer.Deserialize<CartViewModel>(json) ?? new CartViewModel();
        }
        private void ClearCart() => HttpContext.Session.Remove(CART_KEY);
    }
}
