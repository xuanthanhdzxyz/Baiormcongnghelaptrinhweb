using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bai6_Validation.Models
{
    [Table("EventImages_bit240215")]
    public class EventImage_bit240215
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "URL ảnh không được để trống")]
        [Display(Name = "Đường dẫn ảnh")]
        [StringLength(500, ErrorMessage = "URL ảnh tối đa 500 ký tự")]
        public string ImageUrl { get; set; } = string.Empty;

        [Display(Name = "Ảnh đại diện")]
        public bool IsThumbnail { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn sự kiện")]
        [Display(Name = "Sự kiện")]
        public int EventId { get; set; }

        // Quan hệ: thuộc một sự kiện
        [ForeignKey("EventId")]
        public virtual Event_bit240215? Event { get; set; }
    }
}