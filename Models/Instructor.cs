using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class Instructor
    {
        [Key]

      
        public int instructorID { get; set; }
        public string motto { get; set; }
        public string bio { get; set; }
        public string eligible { get; set; }

        // الرصيد المعلق (في انتظار التحويل)
        [Column(TypeName = "decimal(10, 2)")]
        public decimal PendingBalance { get; set; } = 0;

        // الرصيد المتاح للسحب
        [Column(TypeName = "decimal(10, 2)")]
        public decimal AvailableBalance { get; set; } = 0;

        // إجمالي الأرباح على الإطلاق
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalEarnings { get; set; } = 0;

        // إجمالي المسحوبات
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalWithdrawn { get; set; } = 0;
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
