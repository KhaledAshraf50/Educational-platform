using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class AddCourseVM
{
    [Required]
    public string CourseName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    [Required]
    public int ClassID { get; set; }
    public List<SelectListItem> Classes { get; set; }

    [Required]
    public int SubjectId { get; set; }
    public List<SelectListItem> Subjects { get; set; }

    public IFormFile ImageFile { get; set; }

    // محتوى الكورس
    public string NameUrl1 { get; set; }
    public string Url1 { get; set; }
    public string NameUrl2 { get; set; }
    public string Url2 { get; set; }
    public string NameUrl3 { get; set; }
    public string Url3 { get; set; }

    // الامتحانات و المهام
    public List<SelectListItem> Exams { get; set; }
    public List<SelectListItem> Tasks { get; set; }

    public int? SelectedExamId { get; set; }
    public int? SelectedTaskId { get; set; }
}
