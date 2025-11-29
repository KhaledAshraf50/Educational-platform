using Microsoft.AspNetCore.Mvc.Rendering;

namespace Luno_platform.Viewmodel
{
    public class AddCourseVM
    {
        public string CourseName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile ImageFile { get; set; }

        public int SubjectId { get; set; }
        public int ClassID { get; set; }

        // فيديوهات وملفات
        public string NameUrl1 { get; set; }
        public string Url1 { get; set; }
        public string NameUrl2 { get; set; }
        public string Url2 { get; set; }
        public string NameUrl3 { get; set; }
        public string Url3 { get; set; }

        // Dropdowns
        public IEnumerable<SelectListItem> Subjects { get; set; }
        public IEnumerable<SelectListItem> Classes { get; set; }
    }

}
