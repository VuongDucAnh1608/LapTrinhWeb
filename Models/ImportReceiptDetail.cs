using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website_QuanLyKhoHangThucPham.Models
{
    public class ImportReceiptDetail
    {
        public int Id { get; set; }

        public int ImportReceiptId { get; set; }
        public ImportReceipt? ImportReceipt { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Đơn giá")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Hạn sử dụng")]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Ngày sản xuất")]
        public DateTime ManufactureDate { get; set; }

        [MaxLength(100)]
        [Display(Name = "Mã lô hàng")]
        public string? BatchCode { get; set; }

        [NotMapped]
        public decimal SubTotal => Quantity * UnitPrice;
    }
}