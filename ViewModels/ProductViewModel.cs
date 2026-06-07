using System.ComponentModel.DataAnnotations;

namespace Website_QuanLyKhoHangThucPham.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [MaxLength(200)]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        [Display(Name = "Mã SKU")]
        public string? SKU { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn nhà cung cấp")]
        [Display(Name = "Nhà cung cấp")]
        public int SupplierId { get; set; }

        [MaxLength(20)]
        [Display(Name = "Đơn vị tính")]
        public string Unit { get; set; } = "Cái";

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Giá nhập không hợp lệ")]
        [Display(Name = "Giá nhập (₫)")]
        public decimal CostPrice { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Giá bán không hợp lệ")]
        [Display(Name = "Giá bán (₫)")]
        public decimal SellPrice { get; set; }

        [Display(Name = "Tồn kho tối thiểu")]
        public int MinStockLevel { get; set; } = 10;

        [MaxLength(500)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Ảnh sản phẩm")]
        public IFormFile? ImageFile { get; set; }

        public string? ExistingImageUrl { get; set; }
    }
}
