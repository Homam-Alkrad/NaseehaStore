using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NaseehaStore.Models.Data
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generate Id
        public int Id { get; set; }

        [Required(ErrorMessage = "Course name is required.")]
        [StringLength(150, ErrorMessage = "Course name cannot exceed 150 characters.")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; } // Single price field

        [StringLength(255, ErrorMessage = "Image path cannot exceed 255 characters.")]
        public string? ImagePath { get; set; } // Path to the uploaded image
    }
}
