using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Website_QuanLyKhoHangThucPham.Data;
using Website_QuanLyKhoHangThucPham.Models;

namespace Website_QuanLyKhoHangThucPham.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly AppDbContext _db;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "ROLE_PERMISSIONS";

        public PermissionService(AppDbContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
        }

        // Kiểm tra quyền: roles của user + controller + action → có trong DB không?
        public async Task<bool> HasPermissionAsync(IEnumerable<string> roles, string controller, string action)
        {
            // Admin luôn có tất cả quyền
            if (roles.Contains("Admin")) return true;

            var map = await GetCachedMapAsync();

            foreach (var role in roles)
            {
                if (!map.TryGetValue(role, out var permIds)) continue;

                // Tìm permission khớp controller+action
                var perms = await GetAllCachedPermissionsAsync();
                var match = perms.Any(p =>
                    permIds.Contains(p.Id) &&
                    p.Controller.Equals(controller, StringComparison.OrdinalIgnoreCase) &&
                    p.Action.Equals(action, StringComparison.OrdinalIgnoreCase));

                if (match) return true;
            }
            return false;
        }

        public async Task<List<AppPermission>> GetAllAsync()
            => await _db.AppPermissions.OrderBy(p => p.Controller).ThenBy(p => p.Action).ToListAsync();

        public async Task<Dictionary<string, List<int>>> GetRolePermissionMapAsync()
        {
            var data = await _db.RolePermissions.ToListAsync();
            return data.GroupBy(r => r.RoleName)
                       .ToDictionary(g => g.Key, g => g.Select(r => r.PermissionId).ToList());
        }

        // Admin thay đổi quyền của một role — lưu vào DB + xóa cache
        public async Task SetRolePermissionsAsync(string roleName, List<int> permissionIds)
        {
            var existing = _db.RolePermissions.Where(rp => rp.RoleName == roleName);
            _db.RolePermissions.RemoveRange(existing);

            foreach (var pid in permissionIds)
                _db.RolePermissions.Add(new RolePermission { RoleName = roleName, PermissionId = pid });

            await _db.SaveChangesAsync();
            InvalidateCache();
        }

        public void InvalidateCache()
        {
            _cache.Remove(CacheKey);
            _cache.Remove(CacheKey + "_perms");
        }

        // Cache 5 phút để tránh truy vấn DB mỗi request
        private async Task<Dictionary<string, List<int>>> GetCachedMapAsync()
        {
            if (_cache.TryGetValue(CacheKey, out Dictionary<string, List<int>>? cached) && cached != null)
                return cached;

            var map = await GetRolePermissionMapAsync();
            _cache.Set(CacheKey, map, TimeSpan.FromMinutes(5));
            return map;
        }

        private async Task<List<AppPermission>> GetAllCachedPermissionsAsync()
        {
            var key = CacheKey + "_perms";
            if (_cache.TryGetValue(key, out List<AppPermission>? cached) && cached != null)
                return cached;

            var perms = await GetAllAsync();
            _cache.Set(key, perms, TimeSpan.FromMinutes(10));
            return perms;
        }
    }
}
