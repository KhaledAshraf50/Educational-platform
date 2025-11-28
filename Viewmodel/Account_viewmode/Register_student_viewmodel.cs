using System.ComponentModel.DataAnnotations;

namespace Luno_platform.Viewmodel.Account_viewmode
{
    public class Register_student_viewmodel
    {
        [Required(ErrorMessage = "الصف الدراسي مطلوب")]
        [Display(Name = "الصف الدراسي")]
        public int ClassID { get; set; }

        [Required(ErrorMessage = "الفرع مطلوب")]
        [MaxLength(100, ErrorMessage = "اسم الفرع لا يزيد عن 100 حرف")]
        [Display(Name = "الفرع")]
        public string Branch { get; set; }



        [Required(ErrorMessage = "المحافظة مطلوبة")]
        [MaxLength(50, ErrorMessage = "اسم المحافظة لا يزيد عن 50 حرف")]
        [Display(Name = "المحافظة")]
        public string Government { get; set; }

        [Required(ErrorMessage = "المدينة مطلوبة")]
        [MaxLength(50, ErrorMessage = "اسم المدينة لا يزيد عن 50 حرف")]
        [Display(Name = "المدينة")]
        public string City { get; set; }
    }
}
