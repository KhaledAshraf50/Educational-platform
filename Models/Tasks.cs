using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class Tasks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskID { get; set; }

        [Required]
        [MaxLength(200)]
        public string TaskName { get; set; }

        public DateTime createdAT { get; set; }

        public int NumOfQuestions { get; set; }


        public int instructorId { get; set; }

        [ForeignKey("instructorId")]
        public virtual Instructor Instructor { get; set; }



        public int ClassId { get; set; }

        [ForeignKey("ClassId")]
        public virtual Classes Classes { get; set; }



        public virtual ICollection<Question> Questions { get; set; }



        public virtual CourseContent CourseContent { get; set; }



        public virtual ICollection<StudentAnswer> StudentAnswers { get; set; }
        public virtual ICollection<StudentStatistics> StudentStatistics { get; set; }











    }
}
