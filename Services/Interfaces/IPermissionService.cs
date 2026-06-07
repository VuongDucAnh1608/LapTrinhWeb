using Website_QuanLyKhoHangThucPham.Models;
namespace Website_QuanLyKhoHangThucPham.Services
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(IEnumerable<string> roles, string controller, string action);
        Task<List<AppPermission>> GetAllAsync();
        Task<Dictionary<string, List<int>>> GetRolePermissionMapAsync();
        Task SetRolePermissionsAsync(string roleName, List<int> permissionIds);
        void InvalidateCache();
    }
}
