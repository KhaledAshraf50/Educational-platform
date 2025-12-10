using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Luno_platform.Viewmodel
{
    public class CreateExamVM
    {
        public string ExamName { get; set; }
        public int Time { get; set; } // بالدقائق
        public int ClassId { get; set; }
        public int subjectId { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalMarks
        {
            get; set;
        }
        [Required]
        public int CourseId { get; set; }

        // ✅ Dropdown List
        public List<SelectListItem> Courses { get; set; }
    }

}
