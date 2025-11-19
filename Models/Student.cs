using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class Student
    {
        [Key]
      
        public int StudentID { get; set; }

        [MaxLength(100)]
        public string branch { get; set; }

        public string Image { get; set; }
        [Required]

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }

        public virtual ICollection<Student_Courses> Student_Courses { get; set; }


     
        public int classId { get; set; }

        [ForeignKey("classId")]
        public virtual Classes Classes { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual Parent Parent { get; set; }

        public virtual ICollection<StudentAnswer> StudentAnswers { get; set; }
        public virtual ICollection<Payments> Payments { get; set; }
        public virtual ICollection<StudentStatistics> StudentStatistics { get; set; }




    }
}
