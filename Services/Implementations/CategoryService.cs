using Microsoft.EntityFrameworkCore;
using Website_QuanLyKhoHangThucPham.Data;
using Website_QuanLyKhoHangThucPham.Models;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _db;

        public CategoryService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Category>> GetAllAsync(bool activeOnly = true)
        {
            var query = _db.Categories.AsQueryable();
            if (activeOnly)
                query = query.Where(c => c.IsActive);
            return await query.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _db.Categories.FindAsync(id);
        }

        public async Task<Category> CreateAsync(CategoryViewModel model)
        {
            var category = new Category
            {
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive,
                CreatedAt = DateTime.UtcNow
            };
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateAsync(int id, CategoryViewModel model)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null) return null;

            category.Name = model.Name;
            category.Description = model.Description;
            category.IsActive = model.IsActive;

            await _db.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _db.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return false;
            if (category.Products.Any()) return false;

            category.IsActive = false;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
