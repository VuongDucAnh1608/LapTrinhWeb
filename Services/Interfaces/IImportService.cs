using Website_QuanLyKhoHangThucPham.Models;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Services
{
    public interface IImportService
    {
        Task<PagedResult<ImportReceipt>> GetPagedAsync(
            DateTime? fromDate, DateTime? toDate, int page, int pageSize = 10);
        Task<ImportReceipt?> GetByIdAsync(int id);
        Task<ImportReceipt> CreateAsync(ImportReceiptViewModel model, string userId);
        Task<bool> ConfirmAsync(int id, string userId);
        Task<bool> CancelAsync(int id, string userId);
        Task<List<Supplier>> GetSuppliersAsync();
        Task<List<InventoryBatch>> GetExpiringBatchesAsync(int daysAhead = 30);
    }
}
