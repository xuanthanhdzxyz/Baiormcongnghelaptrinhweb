using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bai6_Validation.Models
{
    [Table("Events_bit240215")]
    public class Event_bit240215
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sự kiện không được để trống")]
        [Display(Name = "Tên sự kiện")]
        [StringLength(200, ErrorMessage = "Tên sự kiện tối đa 200 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá không được nhỏ hơn 0")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá không được nhỏ hơn 0")]
        [Display(Name = "Giá (VND)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu không được để trống")]
        [Display(Name = "Ngày bắt đầu")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc không được để trống")]
        [Display(Name = "Ngày kết thúc")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Địa điểm không được để trống")]
        [Display(Name = "Địa điểm")]
        [StringLength(200, ErrorMessage = "Địa điểm tối đa 200 ký tự")]
        public string Location { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        [StringLength(1000, ErrorMessage = "Mô tả tối đa 1000 ký tự")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn loại sự kiện")]
        [Display(Name = "Loại sự kiện")]
        public int EventCategoryId { get; set; }

        // Quan hệ: thuộc một loại
        [ForeignKey("EventCategoryId")]
        public virtual EventCategory_bit240215? EventCategory { get; set; }

        // Quan hệ: có nhiều ảnh
        public virtual ICollection<EventImage_bit240215>? EventImages { get; set; }
    }
}