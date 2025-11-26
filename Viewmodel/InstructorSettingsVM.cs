namespace Luno_platform.Viewmodel
{
    public class InstructorSettingsVM
    {
        // بيانات المدرس الأساسية
        public int InstructorID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string SubjectName { get; set; }
        public string AvatarUrl { get; set; }

        // لتغيير الصورة
        public IFormFile AvatarFile { get; set; }

        // لتغيير كلمة المرور
        public string CurrentPassword { get; set; }
        public string ConfirmCurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

        // الإشعارات
        public List<string> Notifications { get; set; } = new List<string>();

        // إعدادات الإشعارات (مثلاً: إشعارات الدفع، التقييمات، إلخ)
        public List<NotificationSetting> NotificationSettings { get; set; } = new List<NotificationSetting>
        {
            new NotificationSetting { Label = "إشعارات الدفعات الجديدة", IsEnabled = true },
            new NotificationSetting { Label = "إشعارات تقييمات الطلاب", IsEnabled = true },
            new NotificationSetting { Label = "إشعارات طلبات الانضمام للكورسات", IsEnabled = true },
            new NotificationSetting { Label = "تذكيرات بالمهام والامتحانات", IsEnabled = false }
        };
    }

    public class NotificationSetting
    {
        public string Label { get; set; }
        public bool IsEnabled { get; set; }
    }

}
