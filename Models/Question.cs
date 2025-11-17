using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class Question
    {
        [Key]
        public int questionID { get; set; }
        [Required]
        public int degree { get; set; }

        [Required]
        public string questionText { get; set; }

        [Required]
        public string correctAnswer { get; set; }

        [Required]
        public string chooseA { get; set; }

        [Required]
        public string chooseB { get; set; }
        [Required]
        public string chooseC { get; set; }
        [Required]
        public string chooseD { get; set; }

        public int? TaskId { get; set; }

        [ForeignKey("TaskId")]
        public virtual Tasks Tasks { get; set; }

        public int? ExamId { get; set; }

        [ForeignKey("ExamId")]
        public virtual Exams Exams { get; set; }




        public virtual ICollection<StudentAnswer> StudentAnswers { get; set; }


    }
}
