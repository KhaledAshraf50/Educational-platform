using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class StudentStatistics
    {
        [Key]
        public int Statistics_ID { get; set; }

        public int degree { get; set; }

        [Required]
        public int StudentID { get; set; }

        [ForeignKey("StudentID")]
        public virtual Student Student { get; set; }

        public int? ExamId { get; set; }

        [ForeignKey("ExamId")]
        public virtual Exams Exams { get; set; }

        public int? TaskId { get; set; }

        [ForeignKey("TaskId")]
        public virtual Tasks Tasks { get; set; }
    }


}
