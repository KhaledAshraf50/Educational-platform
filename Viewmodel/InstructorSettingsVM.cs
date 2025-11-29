namespace Luno_platform.Viewmodel
{
    public class InstructorSettingsVM
    {
        // بيانات المدرس الأساسية
        public int InstructorID { get; set; }
        public int UserId { get; set; } 

        // بيانات من جدول Users
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public IFormFile ImageFile { get; set; }

        // بيانات من جدول Instructor
        public string Motto { get; set; }
        public string Bio { get; set; }
        public string Eligible { get; set; }

        // المادة
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public string AvatarUrl { get; set; }

        // إشعارات

        public List<NotificationSettingVM> NotificationSettings { get; set; } = new();
        // الحقول الخاصة بتغيير كلمة المرور
        public string CurrentPassword { get; set; }
        public string ConfirmCurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

        // الإشعارات
        public List<string> Notifications { get; set; } = new List<string>();

        // إعدادات الإشعارات (مثلاً: إشعارات الدفع، التقييمات، إلخ)
        public List<NotificationSettingVM> NotificationSetting { get; set; } = new List<NotificationSettingVM>
        {
            new NotificationSettingVM { Label = "إشعارات الدفعات الجديدة", IsEnabled = true },
            new NotificationSettingVM { Label = "إشعارات تقييمات الطلاب", IsEnabled = true },
            new NotificationSettingVM { Label = "إشعارات طلبات الانضمام للكورسات", IsEnabled = true },
            new NotificationSettingVM { Label = "تذكيرات بالمهام والامتحانات", IsEnabled = false }
        };
    }

    public class NotificationSettingVM
    {
        public string Label { get; set; }
        public bool IsEnabled { get; set; }
    }


}
