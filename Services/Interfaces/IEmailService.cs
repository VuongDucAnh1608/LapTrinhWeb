namespace Website_QuanLyKhoHangThucPham.Services
{
    public interface IEmailService
    {
        Task SendAsync(string toEmail, string subject, string htmlBody);
        Task SendPasswordResetAsync(string toEmail, string resetLink);
    }
}
