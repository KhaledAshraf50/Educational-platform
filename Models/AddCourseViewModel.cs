namespace Luno_platform.Models
{
    public class AddCourseViewModel
    {
        public string Title { get; set; }
        public int Grade { get; set; }
        public int SubjectId { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public IFormFile ?ImageFile { get; set; }

        public List<Classes> Classes { get; set; }
        public List<Subject> SubjectsList { get; set; }
    }
}
