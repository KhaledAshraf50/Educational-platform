using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class Instructor
    {
        [Key]
       
        public int instructorID { get; set; }

        [Required]

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }



        public virtual ICollection<Courses>courses { get; set; }
   


        public int SubjectID { get; set; }

        [ForeignKey("SubjectID")]
        public virtual Subject Subject { get; set; }


        public virtual ICollection<instructor_classescs> instructor_classescs { get; set; }
        public virtual ICollection<Exams> Exams { get; set; }
        //public virtual ICollection<Tasks> Tasks { get; set; }
        public virtual ICollection<Teacher_payment> Teacher_Payments { get; set; }
        public virtual ICollection<Tasks> Tasks { get; set; }




    }

}
