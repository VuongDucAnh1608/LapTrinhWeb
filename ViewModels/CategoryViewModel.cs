using System.ComponentModel.DataAnnotations;

namespace Website_QuanLyKhoHangThucPham.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [MaxLength(150)]
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
