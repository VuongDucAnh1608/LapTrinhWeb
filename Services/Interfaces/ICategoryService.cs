using Website_QuanLyKhoHangThucPham.Models;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Services
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync(bool activeOnly = true);
        Task<Category?> GetByIdAsync(int id);
        Task<Category> CreateAsync(CategoryViewModel model);
        Task<Category?> UpdateAsync(int id, CategoryViewModel model);
        Task<bool> DeleteAsync(int id);
    }
}
