using Microsoft.EntityFrameworkCore;
using Website_QuanLyKhoHangThucPham.Data;
using Website_QuanLyKhoHangThucPham.Models;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Services
{
    public class ExportService : IExportService
    {
        private readonly AppDbContext _db;
        private readonly IAuditService _auditService;

        public ExportService(AppDbContext db, IAuditService auditService)
        {
            _db = db;
            _auditService = auditService;
        }

        public async Task<PagedResult<ExportRequest>> GetPagedAsync(
            ExportStatus? status, int page, int pageSize = 10)
        {
            var query = _db.ExportRequests
                .Include(r => r.RequestedBy)
                .Include(r => r.ProcessedBy)
                .Include(r => r.Details)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(r => r.RequestDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ExportRequest>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<ExportRequest>> GetByUserAsync(
            string userId, int page, int pageSize = 10)
        {
            var total = await _db.ExportRequests
                .CountAsync(r => r.RequestedById == userId);

            var items = await _db.ExportRequests
                .Include(r => r.Details)
                    .ThenInclude(d => d.Product)
                .Include(r => r.ProcessedBy)
                .Where(r => r.RequestedById == userId)
                .OrderByDescending(r => r.RequestDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ExportRequest>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<ExportRequest?> GetByIdAsync(int id)
        {
            return await _db.ExportRequests
                .Include(r => r.RequestedBy)
                .Include(r => r.ProcessedBy)
                .Include(r => r.Details)
                    .ThenInclude(d => d.Product)
                        .ThenInclude(p => p!.Batches)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ExportRequest> CreateAsync(ExportRequestViewModel model, string userId)
        {
            var request = new ExportRequest
            {
                RequestCode = GenerateCode("EXP"),
                RequestedById = userId,
                Note = model.Note,
                Status = ExportStatus.Pending,
                RequestDate = DateTime.UtcNow,
                Details = model.Details.Select(d => new ExportRequestDetail
                {
                    ProductId = d.ProductId,
                    RequestedQuantity = d.RequestedQuantity,
                    Note = d.Note
                }).ToList()
            };

            _db.ExportRequests.Add(request);
            await _db.SaveChangesAsync();

            await _auditService.LogAsync(userId, "CREATE_EXPORT",
                "ExportRequest", request.Id.ToString(), null,
                $"Gửi yêu cầu xuất hàng {request.RequestCode}");

            return request;
        }

        public async Task<bool> ApproveAsync(int id, ProcessExportViewModel model, string processorId)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var request = await _db.ExportRequests
                    .Include(r => r.Details)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (request == null || request.Status != ExportStatus.Pending)
                    return false;

                foreach (var item in model.Items)
                {
                    var detail = request.Details.FirstOrDefault(d => d.Id == item.DetailId);
                    if (detail == null) continue;

                    detail.ActualQuantity = item.ActualQuantity;

                    int remainingToDeduct = item.ActualQuantity;
                    var batches = await _db.InventoryBatches
                        .Where(b => b.ProductId == detail.ProductId
                                 && b.Quantity > 0
                                 && b.ExpiryDate > DateTime.UtcNow)
                        .OrderBy(b => b.ExpiryDate)
                        .ToListAsync();

                    foreach (var batch in batches)
                    {
                        if (remainingToDeduct <= 0) break;
                        int deduct = Math.Min(batch.Quantity, remainingToDeduct);
                        batch.Quantity -= deduct;
                        remainingToDeduct -= deduct;
                    }
                }

                request.Status = ExportStatus.Completed;
                request.ProcessedById = processorId;
                request.ProcessedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                await _auditService.LogAsync(processorId, "APPROVE_EXPORT",
                    "ExportRequest", id.ToString(), null,
                    $"Duyệt xuất hàng {request.RequestCode}");

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> RejectAsync(int id, string reason, string processorId)
        {
            var request = await _db.ExportRequests.FindAsync(id);
            if (request == null || request.Status != ExportStatus.Pending)
                return false;

            request.Status = ExportStatus.Rejected;
            request.ProcessedById = processorId;
            request.ProcessedAt = DateTime.UtcNow;
            request.RejectionReason = reason;

            await _db.SaveChangesAsync();

            await _auditService.LogAsync(processorId, "REJECT_EXPORT",
                "ExportRequest", id.ToString(), null,
                $"Từ chối yêu cầu xuất {request.RequestCode}: {reason}");

            return true;
        }

        public async Task<List<Product>> GetAvailableProductsAsync()
        {
            return await _db.Products
                .Include(p => p.Batches)
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        private static string GenerateCode(string prefix)
            => $"{prefix}-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
    }
}
