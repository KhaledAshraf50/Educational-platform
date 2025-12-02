namespace Luno_platform.Viewmodel
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int SubjectId { get; set; }
        public int Level { get; set; }
        public DateTime CreatedAt { get; set; }
        public string InstructorName { get; set; }
    }
}
