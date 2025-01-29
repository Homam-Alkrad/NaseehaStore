using System.ComponentModel.DataAnnotations;

namespace NaseehaStore.Models.Data
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "الاسم الكامل مطلوب.")]
        [StringLength(100, ErrorMessage = "الاسم الكامل يجب أن لا يزيد عن 100 حرف.")]
        [MinLength(3, ErrorMessage = "الاسم الكامل يجب أن يكون ٣ أحرف على الأقل.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "رقم الهاتف الرئيسي مطلوب.")]
        [Phone(ErrorMessage = "رقم الهاتف الرئيسي غير صالح.")]
        [MinLength(9, ErrorMessage = "رقم الهاتف الرئيسي يجب أن يحتوي على ٩ أرقام على الأقل.")]
        [RegularExpression(@"^07\d{8}$", ErrorMessage = "رقم الهاتف الرئيسي يجب أن يبدأ بـ 07 ويتبعه 8 أرقام.")]
        public string MainPhone { get; set; }

        [Phone(ErrorMessage = "رقم الهاتف الإضافي غير صالح.")]
        [MinLength(9, ErrorMessage = "رقم الهاتف الإضافي يجب أن يحتوي على ٩ أرقام على الأقل.")]
        [RegularExpression(@"^07\d{8}$", ErrorMessage = "رقم الهاتف الإضافي يجب أن يبدأ بـ 07 ويتبعه 8 أرقام.")]
        public string SecondPhone { get; set; }


        [Required(ErrorMessage = "المدينة مطلوبة.")]
        [StringLength(50, ErrorMessage = "اسم المدينة يجب أن لا يزيد عن 50 حرف.")]
        public string City { get; set; }

        [StringLength(200, ErrorMessage = "الموقع يجب أن لا يزيد عن 200 حرف.")]
        public string Location { get; set; }

        [StringLength(500, ErrorMessage = "تفاصيل السكن يجب أن لا تزيد عن 500 حرف.")]
        public string? Details { get; set; }
    }
}
