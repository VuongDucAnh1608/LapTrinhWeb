using System.ComponentModel.DataAnnotations;

namespace Website_QuanLyKhoHangThucPham.Models
{
    public class InventoryBatch
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Mã lô hàng")]
        public string BatchCode { get; set; } = string.Empty;

        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Display(Name = "Ngày sản xuất")]
        public DateTime ManufactureDate { get; set; }

        [Display(Name = "Hạn sử dụng")]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Ngày nhập kho")]
        public DateTime ReceivedDate { get; set; } = DateTime.UtcNow;

        public bool IsExpiringSoon =>
            ExpiryDate <= DateTime.UtcNow.AddDays(30) && ExpiryDate > DateTime.UtcNow;

        public bool IsExpired => ExpiryDate <= DateTime.UtcNow;

        public int DaysUntilExpiry =>
            (int)(ExpiryDate - DateTime.UtcNow).TotalDays;
    }
}