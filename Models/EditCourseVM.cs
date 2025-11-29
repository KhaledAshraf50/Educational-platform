
namespace Luno_platform.Models
{
    public class EditCourseVM
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }

        // محتوى الكورس
        public string NameUrl1 { get; set; }
        public string Url1 { get; set; }

        public string NameUrl2 { get; set; }
        public string Url2 { get; set; }

        public string NameUrl3 { get; set; }
        public string Url3 { get; set; }

        // امتحان
        public int? ExamID { get; set; }
        public string ExamName { get; set; }

        // واجب
        public int? TaskID { get; set; }
        public string TaskName { get; set; }
    }
}
