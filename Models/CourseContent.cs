using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luno_platform.Models
{
    public class CourseContent
    {
      
        [Key]
        public int Id { get; set; }
        [MaxLength(1000)]
        public string? Url1 { get; set; }
        [MaxLength(1000)]
        public string? Url2 { get; set; }
        [MaxLength(1000)]
        public string? Url3 { get; set; }
        [MaxLength(1000)]
        public string? Url4 { get; set; }
        [MaxLength(1000)]
        public string? Url5 { get; set; }
        [MaxLength(1000)]
        public string? Url6 { get; set; }
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

