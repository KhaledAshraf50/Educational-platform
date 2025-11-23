using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class Classes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ClassID { get; set; }

        [Required]
        [MaxLength(200)]
        public string ClassName { get; set; }
        public virtual ICollection<Courses> Courses { get; set; }

        public virtual ICollection<instructor_classescs> instructor_classescs { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Tasks> Tasks { get; set; }
        public virtual ICollection<Exams> Exams { get; set; }
        public virtual ICollection<Subject_Classes> Subject_Classes { get; set; }






    }
}
