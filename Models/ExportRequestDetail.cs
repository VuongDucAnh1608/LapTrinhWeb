using System.ComponentModel.DataAnnotations;

namespace Website_QuanLyKhoHangThucPham.Models
{
    public class ExportRequestDetail
    {
        public int Id { get; set; }

        public int ExportRequestId { get; set; }
        public ExportRequest? ExportRequest { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Display(Name = "Số lượng yêu cầu")]
        public int RequestedQuantity { get; set; }

        [Display(Name = "Số lượng xuất thực tế")]
        public int? ActualQuantity { get; set; }

        [MaxLength(200)]
        [Display(Name = "Ghi chú dòng")]
        public string? Note { get; set; }
    }
}