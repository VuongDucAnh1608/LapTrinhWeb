using Website_QuanLyKhoHangThucPham.Models;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Services
{
    public interface IProductService
    {
        Task<PagedResult<Product>> GetPagedAsync(string? search, int? categoryId,
            int page = 1, int pageSize = 10);
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(ProductViewModel model);
        Task<Product?> UpdateAsync(int id, ProductViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<List<object>> SearchSuggestionsAsync(string term);
        Task<List<Category>> GetCategoriesAsync();
        Task<List<Supplier>> GetSuppliersAsync();
    }
}
