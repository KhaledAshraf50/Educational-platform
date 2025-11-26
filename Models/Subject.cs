using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int SubjectID { get; set; }

        [Required]
        [MaxLength(200)]
        public string SubjectNameAR { get; set; }
        //[Required]

        [MaxLength(200)]
        public string SubjectNameEN { get; set; }


        public string Image { get; set; }

        public virtual ICollection<Instructor> Instructors { get; set; }
        public virtual ICollection<Courses> Courses { get; set; }
        public virtual ICollection<Subject_Classes> Subject_Classes { get; set; }


        public virtual ICollection<Exams> Exams { get; set; }




    }
}
