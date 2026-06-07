namespace Website_QuanLyKhoHangThucPham.Services
{
    public interface ICloudinaryService
    {
        Task<CloudinaryUploadResult> UploadAsync(IFormFile file, string folder = "products");
        Task<bool> DeleteAsync(string publicId);
    }

    public class CloudinaryUploadResult
    {
        public bool Success { get; set; }
        public string Url { get; set; } = string.Empty;
        public string PublicId { get; set; } = string.Empty;
        public string? Error { get; set; }
    }
}
