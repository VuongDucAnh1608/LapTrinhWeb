using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Website_QuanLyKhoHangThucPham.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration config)
        {
            var account = new Account(
                config["Cloudinary:CloudName"],
                config["Cloudinary:ApiKey"],
                config["Cloudinary:ApiSecret"]
            );
            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        public async Task<CloudinaryUploadResult> UploadAsync(IFormFile file, string folder = "products")
        {
            await using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File           = new FileDescription(file.FileName, stream),
                Folder         = $"warehouse/{folder}",
                Transformation = new Transformation().Width(800).Height(800)
                                                     .Crop("limit").Quality("auto").FetchFormat("auto"),
                UniqueFilename = true,
                Overwrite      = false
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
                return new CloudinaryUploadResult { Success = false, Error = result.Error.Message };

            return new CloudinaryUploadResult
            {
                Success  = true,
                Url      = result.SecureUrl.ToString(),
                PublicId = result.PublicId
            };
        }

        public async Task<bool> DeleteAsync(string publicId)
        {
            var result = await _cloudinary.DestroyAsync(
                new DeletionParams(publicId) { ResourceType = ResourceType.Image });
            return result.Result == "ok";
        }
    }
}
