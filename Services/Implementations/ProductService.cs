using Microsoft.EntityFrameworkCore;
using Website_QuanLyKhoHangThucPham.Data;
using Website_QuanLyKhoHangThucPham.Models;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;
        private readonly ICloudinaryService _cloudinary;

        public ProductService(AppDbContext db, ICloudinaryService cloudinary)
        {
            _db        = db;
            _cloudinary = cloudinary;
        }

        public async Task<PagedResult<Product>> GetPagedAsync(string? search, int? categoryId, int page = 1, int pageSize = 10)
        {
            var q = _db.Products.Include(p => p.Category).Include(p => p.Supplier)
                                .Include(p => p.Batches).Where(p => p.IsActive);
            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(p => p.Name.Contains(search) || (p.SKU != null && p.SKU.Contains(search)));
            if (categoryId.HasValue) q = q.Where(p => p.CategoryId == categoryId);
            var total = await q.CountAsync();
            var items = await q.OrderBy(p => p.Name).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedResult<Product> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
        }

        public async Task<Product?> GetByIdAsync(int id)
            => await _db.Products.Include(p => p.Category).Include(p => p.Supplier)
                                  .Include(p => p.Batches).FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Product> CreateAsync(ProductViewModel model)
        {
            string? imageUrl = null;
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var result = await _cloudinary.UploadAsync(model.ImageFile, "products");
                imageUrl = result.Success ? result.Url : null;
            }

            var product = new Product
            {
                Name = model.Name, SKU = model.SKU, CategoryId = model.CategoryId,
                SupplierId = model.SupplierId, Unit = model.Unit, CostPrice = model.CostPrice,
                SellPrice = model.SellPrice, MinStockLevel = model.MinStockLevel,
                Description = model.Description, ImageUrl = imageUrl,
                IsActive = true, CreatedAt = DateTime.UtcNow
            };
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateAsync(int id, ProductViewModel model)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return null;

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var result = await _cloudinary.UploadAsync(model.ImageFile, "products");
                if (result.Success) product.ImageUrl = result.Url;
            }

            product.Name = model.Name; product.SKU = model.SKU;
            product.CategoryId = model.CategoryId; product.SupplierId = model.SupplierId;
            product.Unit = model.Unit; product.CostPrice = model.CostPrice;
            product.SellPrice = model.SellPrice; product.MinStockLevel = model.MinStockLevel;
            product.Description = model.Description;
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var p = await _db.Products.FindAsync(id);
            if (p == null) return false;
            p.IsActive = false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<object>> SearchSuggestionsAsync(string term)
            => await _db.Products
                .Where(p => p.IsActive && p.Name.Contains(term))
                .Select(p => (object)new { id = p.Id, label = p.Name, unit = p.Unit, price = p.SellPrice })
                .Take(10).ToListAsync();

        public async Task<List<Category>> GetCategoriesAsync()
            => await _db.Categories.Where(c => c.IsActive).OrderBy(c => c.Name).ToListAsync();

        public async Task<List<Supplier>> GetSuppliersAsync()
            => await _db.Suppliers.Where(s => s.IsActive).OrderBy(s => s.Name).ToListAsync();
    }
}
