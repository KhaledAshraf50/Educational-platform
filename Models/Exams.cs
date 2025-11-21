using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class Exams
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExamID { get; set; }

        [Required]
        [MaxLength(200)]
        public string ExamName { get; set; }

        public DateTime createdAT { get; set; }

        public int degreeExam { get; set; }

        public int NumOfQuestions { get; set; }

        public int Time { get; set; }

        public int attempt { get; set; }


        [Required]
        public int instructorID { get; set; }

        [ForeignKey("instructorID")]
        public virtual Instructor Instructor { get; set; }





        public int ClassId { get; set; }

        [ForeignKey("ClassId")]
        public virtual Classes Classess { get; set; }



        public virtual ICollection<Question> Questions { get; set; }

        //________________________________________



        public virtual CourseContent CourseContent { get; set; }


        public virtual ICollection<StudentAnswer> StudentAnswers { get; set; }

        public virtual ICollection<StudentStatistics> StudentStatistics { get; set; }



        public int subjectId { get; set; }

        [ForeignKey("subjectId")]
        public virtual Subject Subject { get; set; }







    }
}
