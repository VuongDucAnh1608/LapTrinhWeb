namespace Website_QuanLyKhoHangThucPham.Services
{
    public interface IAuditService
    {
        Task LogAsync(string? userId, string action, string? entityName,
            string? entityId, string? ipAddress = null, string? description = null);
    }
}
