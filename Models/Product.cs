using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Website_QuanLyKhoHangThucPham.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [MaxLength(200)]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        [Display(Name = "Mã SKU")]
        public string? SKU { get; set; }

        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Display(Name = "Nhà cung cấp")]
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        [MaxLength(20)]
        [Display(Name = "Đơn vị tính")]
        public string Unit { get; set; } = "Cái";

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá nhập")]
        public decimal CostPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Giá bán")]
        public decimal SellPrice { get; set; }

        [Display(Name = "Tồn kho tối thiểu")]
        public int MinStockLevel { get; set; } = 10;

        [Display(Name = "Ảnh sản phẩm")]
        public string? ImageUrl { get; set; }

        [MaxLength(500)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<InventoryBatch> Batches { get; set; } = new List<InventoryBatch>();
        public ICollection<ImportReceiptDetail> ImportDetails { get; set; } = new List<ImportReceiptDetail>();
        public ICollection<ExportRequestDetail> ExportDetails { get; set; } = new List<ExportRequestDetail>();

        [NotMapped]
        public int TotalStock => Batches?.Where(b => !b.IsExpired).Sum(b => b.Quantity) ?? 0;

        [NotMapped]
        public bool IsLowStock => TotalStock <= MinStockLevel;
    }
}