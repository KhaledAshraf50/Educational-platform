using System.ComponentModel.DataAnnotations;

public class AdminSettingVM
{
    public int AdminId { get; set; }
    public string Image { get; set; }  
    [Required(ErrorMessage = "الاسم مطلوب")]
    public string Name { get; set; }

    [Required(ErrorMessage = "الرقم القومي مطلوب")]
    [StringLength(14, MinimumLength = 14, ErrorMessage = "الرقم القومي يجب أن يكون 14 رقم")]
    public string NationalId { get; set; }

    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
    public string Email { get; set; }

    [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
    public string PhoneNumber { get; set; }

    // الحقول الخاصة بتغيير كلمة المرور
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; }

    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "كلمة المرور الجديدة وتأكيدها غير متطابقين")]
    public string ConfirmNewPassword { get; set; }

    // مثال على إضافة حالة الحساب أو الأدوار
    public bool IsActive { get; set; }
}
