using Microsoft.EntityFrameworkCore;
using Website_QuanLyKhoHangThucPham.Data;
using Website_QuanLyKhoHangThucPham.Models;
using Website_QuanLyKhoHangThucPham.ViewModels;

namespace Website_QuanLyKhoHangThucPham.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly AppDbContext _db;

        public SupplierService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Supplier>> GetAllAsync(bool activeOnly = true)
        {
            var query = _db.Suppliers.AsQueryable();
            if (activeOnly)
                query = query.Where(s => s.IsActive);
            return await query.OrderBy(s => s.Name).ToListAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int id)
            => await _db.Suppliers.FindAsync(id);

        public async Task<Supplier> CreateAsync(SupplierViewModel model)
        {
            var supplier = new Supplier
            {
                Name = model.Name,
                Code = model.Code,
                Address = model.Address,
                Phone = model.Phone,
                Email = model.Email,
                ContactPerson = model.ContactPerson,
                IsActive = model.IsActive,
                CreatedAt = DateTime.UtcNow
            };
            _db.Suppliers.Add(supplier);
            await _db.SaveChangesAsync();
            return supplier;
        }

        public async Task<Supplier?> UpdateAsync(int id, SupplierViewModel model)
        {
            var supplier = await _db.Suppliers.FindAsync(id);
            if (supplier == null) return null;

            supplier.Name = model.Name;
            supplier.Code = model.Code;
            supplier.Address = model.Address;
            supplier.Phone = model.Phone;
            supplier.Email = model.Email;
            supplier.ContactPerson = model.ContactPerson;
            supplier.IsActive = model.IsActive;

            await _db.SaveChangesAsync();
            return supplier;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supplier = await _db.Suppliers
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null || supplier.Products.Any()) return false;

            supplier.IsActive = false;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
