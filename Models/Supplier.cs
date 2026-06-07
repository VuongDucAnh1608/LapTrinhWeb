using System.ComponentModel.DataAnnotations;

namespace Website_QuanLyKhoHangThucPham.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên nhà cung cấp không được để trống")]
        [MaxLength(200)]
        [Display(Name = "Tên nhà cung cấp")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(20)]
        [Display(Name = "Mã nhà cung cấp")]
        public string? Code { get; set; }

        [MaxLength(200)]
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        [MaxLength(20)]
        [Display(Name = "Số điện thoại")]
        public string? Phone { get; set; }

        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [MaxLength(100)]
        [Display(Name = "Người liên hệ")]
        public string? ContactPerson { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<ImportReceipt> ImportReceipts { get; set; } = new List<ImportReceipt>();
    }
}