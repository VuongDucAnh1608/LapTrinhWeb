using System.ComponentModel.DataAnnotations;

namespace Website_QuanLyKhoHangThucPham.ViewModels
{
    public class ExportRequestViewModel
    {
        public int Id { get; set; }

        [MaxLength(500)]
        [Display(Name = "Lý do / Ghi chú")]
        public string? Note { get; set; }

        public List<ExportDetailViewModel> Details { get; set; } = new();
    }

    public class ExportDetailViewModel
    {
        [Required]
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        [Display(Name = "Số lượng yêu cầu")]
        public int RequestedQuantity { get; set; }

        [MaxLength(200)]
        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }

        public int AvailableStock { get; set; }
    }

    public class ProcessExportViewModel
    {
        public int RequestId { get; set; }

        [MaxLength(500)]
        public string? RejectionReason { get; set; }

        public List<ActualQuantityViewModel> Items { get; set; } = new();
    }

    public class ActualQuantityViewModel
    {
        public int DetailId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int RequestedQuantity { get; set; }
        public int AvailableStock { get; set; }
        public int ActualQuantity { get; set; }
    }
}
