using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bai6_Validation.Models
{
    [Table("EventCategories_bit240215")]
    public class EventCategory_bit240215
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên loại sự kiện không được để trống")]
        [Display(Name = "Tên loại")]
        [StringLength(100, ErrorMessage = "Tên loại tối đa 100 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Mô tả")]
        [StringLength(500, ErrorMessage = "Mô tả tối đa 500 ký tự")]
        public string? Description { get; set; }

        // Quan hệ: một loại có nhiều sự kiện
        public virtual ICollection<Event_bit240215>? Events { get; set; }
    }
}