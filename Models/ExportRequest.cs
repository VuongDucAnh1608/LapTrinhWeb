using System.ComponentModel.DataAnnotations;

namespace Website_QuanLyKhoHangThucPham.Models
{
    public class ExportRequest
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Display(Name = "Mã yêu cầu")]
        public string RequestCode { get; set; } = string.Empty;

        [Display(Name = "Ngày yêu cầu")]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;

        public string? RequestedById { get; set; }
        public ApplicationUser? RequestedBy { get; set; }

        public string? ProcessedById { get; set; }
        public ApplicationUser? ProcessedBy { get; set; }

        [Display(Name = "Trạng thái")]
        public ExportStatus Status { get; set; } = ExportStatus.Pending;

        [MaxLength(500)]
        [Display(Name = "Lý do / Ghi chú")]
        public string? Note { get; set; }

        [MaxLength(500)]
        [Display(Name = "Lý do từ chối")]
        public string? RejectionReason { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public ICollection<ExportRequestDetail> Details { get; set; } = new List<ExportRequestDetail>();
    }
}