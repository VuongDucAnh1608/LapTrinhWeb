using System.ComponentModel.DataAnnotations;

namespace Website_QuanLyKhoHangThucPham.ViewModels
{
    public class ImportReceiptViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn nhà cung cấp")]
        [Display(Name = "Nhà cung cấp")]
        public int SupplierId { get; set; }

        [Display(Name = "Ghi chú")]
        [MaxLength(500)]
        public string? Note { get; set; }

        public List<ImportDetailViewModel> Details { get; set; } = new();
    }

    public class ImportDetailViewModel
    {
        [Required]
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Đơn giá không được để trống")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn 0")]
        [Display(Name = "Đơn giá (₫)")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập hạn sử dụng")]
        [Display(Name = "Hạn sử dụng")]
        public DateTime ExpiryDate { get; set; } = DateTime.Today.AddMonths(6);

        [Display(Name = "Ngày sản xuất")]
        public DateTime ManufactureDate { get; set; } = DateTime.Today;

        [Display(Name = "Mã lô hàng")]
        public string? BatchCode { get; set; }
    }
}
