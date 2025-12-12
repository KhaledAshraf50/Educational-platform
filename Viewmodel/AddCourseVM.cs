using Luno_platform.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Luno_platform.Viewmodel
{
    public class AddCourseVM
    {
        [Required]
        public string CourseName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        [Required]
        public int ClassID { get; set; }
        public List<Classes> Classes { get; set; }

        [Required]
        public int SubjectId { get; set; }
        public List<Subject> Subjects { get; set; }

        public IFormFile ImageFile { get; set; }

        // محتوى الكورس
        public string NameUrl1 { get; set; }
        public string Url1 { get; set; }
        public string NameUrl2 { get; set; }
        public string Url2 { get; set; }
        public string NameUrl3 { get; set; }
        public string Url3 { get; set; }
        public string Subjectsname { get; set; }
        

        // لربط الامتحانات والمهام الخاصة بالمدرس
        public List<ExamVM2> Exams { get; set; }
        public List<TaskVM> Tasks { get; set; }

        public int? SelectedExamId { get; set; }
        public int? SelectedTaskId { get; set; }



      

    }


}
public class ExamVM2
{
    public int ExamID { get; set; }
    public string ExamName { get; set; }
}

public class TaskVM
{
    public int TaskID { get; set; }
    public string TaskName { get; set; }
}