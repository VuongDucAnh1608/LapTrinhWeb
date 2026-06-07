using Website_QuanLyKhoHangThucPham.Models;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Services
{
    public interface IExportService
    {
        Task<PagedResult<ExportRequest>> GetPagedAsync(
            ExportStatus? status, int page, int pageSize = 10);
        Task<PagedResult<ExportRequest>> GetByUserAsync(
            string userId, int page, int pageSize = 10);
        Task<ExportRequest?> GetByIdAsync(int id);
        Task<ExportRequest> CreateAsync(ExportRequestViewModel model, string userId);
        Task<bool> ApproveAsync(int id, ProcessExportViewModel model, string processorId);
        Task<bool> RejectAsync(int id, string reason, string processorId);
        Task<List<Product>> GetAvailableProductsAsync();
    }
}
