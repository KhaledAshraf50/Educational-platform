using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Luno_platform.Viewmodel
{
    public class Register_User_Viewmode
    {
        // ===========================
        //  بيانات اليوزر الأساسية
        // ===========================
        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [MaxLength(100)]
        public string Fname { get; set; }

        [Required(ErrorMessage = "الاسم الأخير مطلوب")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Phone(ErrorMessage = "رقم هاتف غير صحيح")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "يجب اختيار النوع")]
        public string Role { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "بريد إلكتروني غير صحيح")]
        public string Email { get; set; }

        [MaxLength(14)]
        public string NationalID { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        public string Password { get; set; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
        public string ConfirmPassword { get; set; }

        public string? Image { get; set; } 


        // ===========================
        //  بيانات الطالب
        // ===========================
        //public string? Governorate { get; set; }
        //public string? Admin { get; set; }
        public int? ClassId { get; set; }
        public string? branch { get; set; }

        public  int? parentnumber { get; set; }

        public string? goverment { get; set; }

        public string? city { get; set; }

        //public int? ParentId { get; set; }

        //// ===========================
        //  بيانات المدرس
        // ===========================
        //public IFormFile? TeacherImage { get; set; }
        public string? Motto { get; set; }
        public string? Bio { get; set; }
        public string? Eligible { get; set; }
        public int? SubjectID { get; set; }

        // ===========================
        //  بيانات ولي الأمر
        // ===========================
        //public IFormFile? ParentImage { get; set; }
    }
}