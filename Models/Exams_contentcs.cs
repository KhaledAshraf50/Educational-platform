namespace Luno_platform.Models
{
    public class Exams_contentcs
    {
        public int ExamId { get; set; }
        public virtual Exams Exams { get; set; }

        public int contentid { get; set; }
        public virtual CourseContent CourseContent { get; set; }
    }
}
