using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class CourseContent
    {
      
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public string? nameurl1 { get; set; }

        [MaxLength(1000)]
        [Url]
        public string? Url1 { get; set; }
        public string? nameurl2 { get; set; }

        [MaxLength(1000)]
        [Url]
        public string? Url2 { get; set; }
        public string? nameurl3 { get; set; }

        [MaxLength(1000)]
        [Url]
        public string? Url3 { get; set; }
        [MaxLength(1000)]

        public int cousrsid { get; set; }

        [ForeignKey("cousrsid")]
       
        public Courses courses { get; set; }

       public int? ExamId { get; set; }
       
        [ForeignKey("ExamId")]
        public Exams Exams { get; set; }

        public int? taskId { get; set; }
        
        [ForeignKey("taskId")]
        public Tasks Tasks { get; set; }


    }




}

