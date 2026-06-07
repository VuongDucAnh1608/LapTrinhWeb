namespace Website_QuanLyKhoHangThucPham.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string? EntityName { get; set; }
        public string? EntityId { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? IpAddress { get; set; }
        public string? Description { get; set; }
    }
}