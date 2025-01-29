using NaseehaStore.Models.Data;
using System.ComponentModel.DataAnnotations;

namespace NaseehaStore.Models.ViewModels
{
    public class OrderViewModel
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; }
        public string? CourseDescription { get; set; }

        [Required(ErrorMessage = "السعر مطلوب.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "يجب أن يكون السعر أكبر من 0.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "معلومات الطالب مطلوبة.")]
        public Student Student { get; set; }
    }
}
