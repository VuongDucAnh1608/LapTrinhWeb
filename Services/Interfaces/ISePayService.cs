namespace Website_QuanLyKhoHangThucPham.Services
{
    public interface ISePayService
    {
        string GenerateQrUrl(string orderCode, long amount);
        string GenerateTransferContent(string orderCode);
        bool VerifyWebhook(string rawBody, string signature);
    }
}
