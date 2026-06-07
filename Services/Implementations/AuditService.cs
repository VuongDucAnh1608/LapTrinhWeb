using Microsoft.AspNetCore.Identity;
using Website_QuanLyKhoHangThucPham.Data;
using Website_QuanLyKhoHangThucPham.Models;

namespace Website_QuanLyKhoHangThucPham.Services
{
    public class AuditService : IAuditService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuditService(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task LogAsync(string? userId, string action, string? entityName,
            string? entityId, string? ipAddress = null, string? description = null)
        {
            string userName = "Anonymous";

            if (!string.IsNullOrEmpty(userId))
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                    userName = user.FullName ?? user.Email ?? "Unknown";
            }

            var log = new AuditLog
            {
                UserId = userId,
                UserName = userName,
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                IpAddress = ipAddress,
                Description = description,
                Timestamp = DateTime.UtcNow
            };

            _db.AuditLogs.Add(log);
            await _db.SaveChangesAsync();
        }
    }
}
