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

        public Courses courses { get; set; }

        
        public virtual ICollection<Tasks> Tasks { get; set; }
        public virtual ICollection<Exams_contentcs> Exams_Content { get; set; }
        public virtual ICollection<Task_content> Task_Contents { get; set; }

    }




}

