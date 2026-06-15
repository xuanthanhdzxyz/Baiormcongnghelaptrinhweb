using System.ComponentModel.DataAnnotations;

namespace Bai6_Validation.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Không được để trống")]
        [Display(Name = "Tên sách")]
        public string? Ten { get; set; }

        [Required(ErrorMessage = "Giá phải lớn hơn 0")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        [Display(Name = "Giá")]
        public decimal Gia { get; set; }

        [Display(Name = "Tác giả")]
        public string? TacGia { get; set; }

        [Display(Name = "Nhà xuất bản")]
        public string? NhaXuatBan { get; set; }
    }
}