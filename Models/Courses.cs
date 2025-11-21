using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class Courses
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Courseid { get; set; }

        [Required]
        [MaxLength(200)]
        public string CourseName { get; set; }

        public string description { get; set; }

        [MaxLength(50)]
        public string status { get; set; }

        public decimal price { get; set; }

        public string Image { get; set; }



        public DateTime createdAt { get; set; }

        public int instructorID { get; set; }

        [ForeignKey("instructorID")]
        public Instructor Instructor { get; set; }

        public int classID { get; set; }

        [ForeignKey("classID")]
        public Classes classes { get; set; }

        public virtual ICollection<Student_Courses> Student_Courses { get; set; }

        public virtual  CourseContent CourseContent { get; set; }



     
        public int SubjectId { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject Subjects { get; set; }


        public virtual ICollection<Payments> Payments { get; set; }

    }
}
