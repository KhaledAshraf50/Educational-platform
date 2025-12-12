using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Luno_platform.Viewmodel
{
    public class CreateTaskVM
    {
        [Required]
        public string TaskName { get; set; }

        public int ClassId { get; set; }

        public int TotalQuestions { get; set; }
        [Required]
        public int CourseId { get; set; }

        // ✅ Dropdown List
        public List<SelectListItem> Courses { get; set; }
    }

}
