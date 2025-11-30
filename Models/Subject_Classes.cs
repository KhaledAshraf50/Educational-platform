namespace Luno_platform.Models
{
    public class Subject_Classes
    {
 


        public int subjectId { get; set; }
        public virtual Subject Subject { get; set; }


        public int classId { get; set; }
        public virtual Classes Classes { get; set; }
    }
}
