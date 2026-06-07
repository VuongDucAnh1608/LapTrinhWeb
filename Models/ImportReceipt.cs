using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website_QuanLyKhoHangThucPham.Models
{
    public class ImportReceipt
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Display(Name = "Mã phiếu nhập")]
        public string ReceiptCode { get; set; } = string.Empty;

        [Display(Name = "Ngày nhập")]
        public DateTime ImportDate { get; set; } = DateTime.UtcNow;

        public string? CreatedById { get; set; }
        public ApplicationUser? CreatedBy { get; set; }

        [Display(Name = "Nhà cung cấp")]
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        [MaxLength(500)]
        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Tổng tiền")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Trạng thái")]
        public ReceiptStatus Status { get; set; } = ReceiptStatus.Draft;

        public string? ConfirmedById { get; set; }
        public ApplicationUser? ConfirmedBy { get; set; }
        public DateTime? ConfirmedAt { get; set; }

        public ICollection<ImportReceiptDetail> Details { get; set; } = new List<ImportReceiptDetail>();
    }
}