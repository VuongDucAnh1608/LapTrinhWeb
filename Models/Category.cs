using System.ComponentModel.DataAnnotations;

namespace Website_QuanLyKhoHangThucPham.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [MaxLength(150, ErrorMessage = "Tên danh mục tối đa 150 ký tự")]
        [Display(Name = "Tên danh mục")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}