namespace Luno_platform.Models
{
    public class instructor_classescs
    {
        public int instructorId { get; set; }
        public virtual Instructor Instructor { get; set; }

        public int classId { get; set; }
        public virtual Classes classes { get; set; }
    }
}
