using NaseehaStore.Models.Data;
using System.ComponentModel.DataAnnotations;

namespace NaseehaStore.Models.ViewModels
{
    public class EditOrderViewModel
    {
        public int OrderId { get; set; }

        [Required]
        public int CourseId { get; set; }

        public string CourseName { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public Student Student { get; set; }
    }
}
