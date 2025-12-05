namespace Luno_platform.Viewmodel
{
    public class showadetailsusers
    {
        // بيانات أساسية من Users
        public int? userid { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? nationalID { get; set; }
        public string? role { get; set; } // Student, Teacher, Parent, Admin
        public DateOnly? createat { get; set; }
        public string? Image { get; set; }

        // بيانات الطالب
        public int? StudentID { get; set; }
        public string? branch { get; set; }
        public int? parentnumber { get; set; }
        public string? goverment { get; set; }
        public string? city { get; set; }
        public int? classId { get; set; }
        public int? ParentId { get; set; }

        // بيانات المعلم
        public int? instructorID { get; set; }
        public string? motto { get; set; }
        public string? bio { get; set; }
        public string? eligible { get; set; }
        public decimal? PendingBalance { get; set; }
        public decimal? AvailableBalance { get; set; }
        public decimal? TotalEarnings { get; set; }
        public decimal? TotalWithdrawn { get; set; }
        public int? SubjectID { get; set; }

        // بيانات ولي الأمر
        public int? ParentID { get; set; }
        public int? ChildrenCount { get; set; }

        // بيانات الأدمن
        public int? AdminID { get; set; }

        // أي بيانات أخرى تريد إضافتها
    }
}
