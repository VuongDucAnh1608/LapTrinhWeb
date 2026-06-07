using Website_QuanLyKhoHangThucPham.Models;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Services
{
    public interface ISupplierService
    {
        Task<List<Supplier>> GetAllAsync(bool activeOnly = true);
        Task<Supplier?> GetByIdAsync(int id);
        Task<Supplier> CreateAsync(SupplierViewModel model);
        Task<Supplier?> UpdateAsync(int id, SupplierViewModel model);
        Task<bool> DeleteAsync(int id);
    }
}
