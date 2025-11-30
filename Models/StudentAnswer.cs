using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class StudentAnswer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        public string studentanswer { get; set; }


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

        public int? QuestionId { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }


    }
}
