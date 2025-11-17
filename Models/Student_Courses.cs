namespace Luno_platform.Models
{
    public class Student_Courses
    {
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public int CourseId { get; set; }
        public virtual Courses Course { get; set; }
    }
}
