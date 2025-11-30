using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Luno_platform.Viewmodel
{
    public class Register_User_Viewmode
    {
        // ===========================
        // بيانات المستخدم الأساسية
        // ===========================
        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [MaxLength(100, ErrorMessage = "الاسم الأول لا يجب أن يزيد عن 100 حرف")]
        public string Fname { get; set; }

        [Required(ErrorMessage = "الاسم الأخير مطلوب")]
        [MaxLength(100, ErrorMessage = "الاسم الأخير لا يجب أن يزيد عن 100 حرف")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "رقم الهاتف يجب أن يبدأ ب01 ويكون 11 رقم")]
        public string PhoneNumber { get; set; }

        //[Required(ErrorMessage = "يجب اختيار النوع")]
        [RegularExpression("student|instructor|parent", ErrorMessage = "النوع غير صحيح")]
        public string? Role { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "بريد إلكتروني غير صحيح")]
        [MaxLength(150, ErrorMessage = "البريد الإلكتروني طويل جدًا")]
        public string Email { get; set; }

        [Required(ErrorMessage = "الرقم القومي موجود من قبل ")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "الرقم القومي يجب أن يكون 14 رقم")]
        public string NationalID { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون 6 أحرف على الأقل")]
        [MaxLength(50, ErrorMessage = "كلمة المرور طويلة جدًا")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string? Image { get; set; }
        public string? Passwordregiester { get; set; }
        

        // ===========================
        // بيانات الطالب
        // ===========================
        [Range(1, 3, ErrorMessage = "الصف الدراسي غير صحيح")]
        public int? ClassId { get; set; }

        [RegularExpression(@"^(science|arts|math|science-sc|science-ma)?$", ErrorMessage = "الشعبة غير صحيحة")]
        public string? branch { get; set; }

        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "رقم هاتف ولي الأمر غير صحيح")]
        public int? parentnumber { get; set; }

        [MaxLength(100, ErrorMessage = "اسم المحافظة طويل جدًا")]
        public string? goverment { get; set; }

        [MaxLength(100, ErrorMessage = "اسم الإدارة طويل جدًا")]
        public string? city { get; set; }

        // ===========================
        // بيانات المدرس
        // ===========================
        [MaxLength(100, ErrorMessage = "الشعار طويل جدًا")]
        public string? Motto { get; set; }

        [MaxLength(500, ErrorMessage = "نبذة عنك لا يجب أن تتجاوز 500 حرف")]
        public string? Bio { get; set; }

        [MaxLength(150, ErrorMessage = "المؤهل طويل جدًا")]
        public string? Eligible { get; set; }

        [Range(1, 20, ErrorMessage = "المادة التخصص غير صحيحة")]
        public int? SubjectID { get; set; }

        // ===========================
        // بيانات ولي الأمر
        // ===========================
        // ممكن تضيف أي حقل مطلوب مع validation هنا
    }
}
