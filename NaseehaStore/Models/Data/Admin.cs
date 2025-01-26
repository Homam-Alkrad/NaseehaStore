using System.ComponentModel.DataAnnotations;

namespace NaseehaStore.Models.Data
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; } // Plaintext password
    }
}
