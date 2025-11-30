namespace Luno_platform.Viewmodel
{
    public class CourseDetailsViewModel
    {
        public string CourseImage { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public decimal Price { get; set; }
        public int StudentsCount { get; set; } // عدد الطلاب المشتركين
        public string Description { get; set; }
        public int CourseId { get; set; } // لإجراء عمليات التعديل والحذف
        public int TeacherId { get; set; } // لمعرفة المدرس

        public List<WeekViewModel> Weeks { get; set; } = new List<WeekViewModel>();
    }

    // نموذج لتمثيل "الأسبوع" في الـ View.
    // سنستخدم كيان CourseContent كـ "أسبوع" لتبسيط الهيكلة المطلوبة في الـ View.
    public class WeekViewModel
    {
        public int WeekId { get; set; } // سيكون هو CourseContent.Id
        public string WeekName { get; set; } // سيكون هو CourseContent.nameurl1
        public List<LessonViewModel> Lessons { get; set; } = new List<LessonViewModel>();
    }

    // نموذج لتمثيل "الدرس" في الـ View.
    // سنستخدم حقول الروابط (Url1, Url2, Url3) كـ "دروس".
    public class LessonViewModel
    {
        public int LessonId { get; set; } // يمكن أن يكون ترقيم تسلسلي فريد للكورس
        public string LessonTitle { get; set; }
    }
}
