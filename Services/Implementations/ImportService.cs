using Microsoft.EntityFrameworkCore;
using Website_QuanLyKhoHangThucPham.Data;
using Website_QuanLyKhoHangThucPham.Models;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Services
{
    public class ImportService : IImportService
    {
        private readonly AppDbContext _db;
        private readonly IAuditService _auditService;

        public ImportService(AppDbContext db, IAuditService auditService)
        {
            _db = db;
            _auditService = auditService;
        }

        public async Task<PagedResult<ImportReceipt>> GetPagedAsync(
            DateTime? fromDate, DateTime? toDate, int page, int pageSize = 10)
        {
            var query = _db.ImportReceipts
                .Include(r => r.Supplier)
                .Include(r => r.CreatedBy)
                .Include(r => r.Details)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(r => r.ImportDate >= fromDate.Value);
            if (toDate.HasValue)
                query = query.Where(r => r.ImportDate <= toDate.Value.AddDays(1));

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(r => r.ImportDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ImportReceipt>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<ImportReceipt?> GetByIdAsync(int id)
        {
            return await _db.ImportReceipts
                .Include(r => r.Supplier)
                .Include(r => r.CreatedBy)
                .Include(r => r.ConfirmedBy)
                .Include(r => r.Details)
                    .ThenInclude(d => d.Product)
                        .ThenInclude(p => p!.Category)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ImportReceipt> CreateAsync(ImportReceiptViewModel model, string userId)
        {
            var receipt = new ImportReceipt
            {
                ReceiptCode = GenerateCode("IMP"),
                SupplierId = model.SupplierId,
                Note = model.Note,
                CreatedById = userId,
                Status = ReceiptStatus.Draft,
                ImportDate = DateTime.UtcNow,
                Details = model.Details.Select(d => new ImportReceiptDetail
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    ExpiryDate = d.ExpiryDate,
                    ManufactureDate = d.ManufactureDate,
                    BatchCode = string.IsNullOrEmpty(d.BatchCode)
                        ? GenerateCode("BATCH")
                        : d.BatchCode
                }).ToList()
            };

            receipt.TotalAmount = receipt.Details.Sum(d => d.Quantity * d.UnitPrice);

            _db.ImportReceipts.Add(receipt);
            await _db.SaveChangesAsync();

            await _auditService.LogAsync(userId, "CREATE_IMPORT",
                "ImportReceipt", receipt.Id.ToString(), null,
                $"Lập phiếu nhập {receipt.ReceiptCode}");

            return receipt;
        }

        public async Task<bool> ConfirmAsync(int id, string userId)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var receipt = await _db.ImportReceipts
                    .Include(r => r.Details)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (receipt == null || receipt.Status != ReceiptStatus.Draft)
                    return false;

                foreach (var detail in receipt.Details)
                {
                    var existingBatch = await _db.InventoryBatches
                        .FirstOrDefaultAsync(b => b.BatchCode == detail.BatchCode);

                    if (existingBatch != null)
                    {
                        existingBatch.Quantity += detail.Quantity;
                    }
                    else
                    {
                        _db.InventoryBatches.Add(new InventoryBatch
                        {
                            ProductId = detail.ProductId,
                            BatchCode = detail.BatchCode!,
                            Quantity = detail.Quantity,
                            ExpiryDate = detail.ExpiryDate,
                            ManufactureDate = detail.ManufactureDate,
                            ReceivedDate = DateTime.UtcNow
                        });
                    }
                }

                receipt.Status = ReceiptStatus.Confirmed;
                receipt.ConfirmedById = userId;
                receipt.ConfirmedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                await _auditService.LogAsync(userId, "CONFIRM_IMPORT",
                    "ImportReceipt", id.ToString(), null,
                    $"Xác nhận phiếu nhập {receipt.ReceiptCode}");

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> CancelAsync(int id, string userId)
        {
            var receipt = await _db.ImportReceipts.FindAsync(id);
            if (receipt == null || receipt.Status != ReceiptStatus.Draft) return false;

            receipt.Status = ReceiptStatus.Cancelled;
            await _db.SaveChangesAsync();

            await _auditService.LogAsync(userId, "CANCEL_IMPORT",
                "ImportReceipt", id.ToString());
            return true;
        }

        public async Task<List<Supplier>> GetSuppliersAsync()
            => await _db.Suppliers.Where(s => s.IsActive).OrderBy(s => s.Name).ToListAsync();

        public async Task<List<InventoryBatch>> GetExpiringBatchesAsync(int daysAhead = 30)
        {
            var threshold = DateTime.UtcNow.AddDays(daysAhead);
            return await _db.InventoryBatches
                .Include(b => b.Product)
                    .ThenInclude(p => p!.Category)
                .Where(b => b.ExpiryDate <= threshold
                         && b.ExpiryDate > DateTime.UtcNow
                         && b.Quantity > 0)
                .OrderBy(b => b.ExpiryDate)
                .ToListAsync();
        }

        private static string GenerateCode(string prefix)
            => $"{prefix}-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
    }
}
