using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Website_QuanLyKhoHangThucPham.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(string toEmail, string subject, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                _config["EmailSettings:SenderName"],
                _config["EmailSettings:SenderEmail"]
            ));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(
                _config["EmailSettings:Host"],
                int.Parse(_config["EmailSettings:Port"] ?? "587"),
                SecureSocketOptions.StartTls
            );
            await client.AuthenticateAsync(
                _config["EmailSettings:SenderEmail"],
                _config["EmailSettings:Password"]
            );
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendPasswordResetAsync(string toEmail, string resetLink)
        {
            string htmlBody = $@"
                <div style='font-family:Arial,sans-serif;max-width:600px;margin:auto;padding:24px;border:1px solid #e0e0e0;border-radius:8px;'>
                    <h2 style='color:#2c3e50;'>Đặt lại mật khẩu</h2>
                    <p>Bạn đã yêu cầu đặt lại mật khẩu cho tài khoản Warehouse Management.</p>
                    <p>Nhấn vào nút bên dưới để đặt lại mật khẩu. Liên kết có hiệu lực trong <strong>24 giờ</strong>.</p>
                    <div style='text-align:center;margin:32px 0;'>
                        <a href='{resetLink}'
                           style='background:#3498db;color:#fff;padding:12px 32px;border-radius:6px;text-decoration:none;font-size:16px;'>
                           Đặt lại mật khẩu
                        </a>
                    </div>
                    <p style='color:#999;font-size:12px;'>Nếu bạn không yêu cầu điều này, hãy bỏ qua email này.</p>
                </div>";

            await SendAsync(toEmail, "Đặt lại mật khẩu — Warehouse Management", htmlBody);
        }
    }
}
