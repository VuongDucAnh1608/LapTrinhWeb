using System.ComponentModel.DataAnnotations;

namespace Website_QuanLyKhoHangThucPham.ViewModels
{
    public class ProductSearchViewModel
    {
        [Display(Name = "Từ khóa")]
        public string? Keyword { get; set; }

        [Display(Name = "Danh mục")]
        public int? CategoryId { get; set; }

        [Display(Name = "Nhà cung cấp")]
        public int? SupplierId { get; set; }

        [Display(Name = "Giá từ")]
        public decimal? MinPrice { get; set; }

        [Display(Name = "Giá đến")]
        public decimal? MaxPrice { get; set; }

        [Display(Name = "Sắp xếp")]
        public string? SortBy { get; set; }

        [Display(Name = "Thứ tự")]
        public string? SortOrder { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
