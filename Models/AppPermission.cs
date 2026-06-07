using System.ComponentModel.DataAnnotations;
namespace Website_QuanLyKhoHangThucPham.Models
{
    public class AppPermission
    {
        public int Id { get; set; }
        [MaxLength(80)] public string PermissionKey { get; set; } = string.Empty;
        [MaxLength(120)] public string DisplayName { get; set; } = string.Empty;
        [MaxLength(60)] public string Controller { get; set; } = string.Empty;
        [MaxLength(60)] public string Action { get; set; } = string.Empty;
        [MaxLength(200)] public string? Description { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }

    public class RolePermission
    {
        public int Id { get; set; }
        [MaxLength(256)] public string RoleName { get; set; } = string.Empty;
        public int PermissionId { get; set; }
        public AppPermission Permission { get; set; } = null!;
    }
}
