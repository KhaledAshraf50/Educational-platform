namespace Luno_platform.Models
{
    public class Task_content
    {
        public int TaskId { get; set; }
        public virtual Tasks Tasks { get; set; }

        public int contentid { get; set; }
        public virtual CourseContent CourseContent { get; set; }
    }
}
