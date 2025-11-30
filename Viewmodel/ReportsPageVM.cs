using Luno_platform.Models;

namespace Luno_platform.Viewmodel
{
    public class ReportsPageVM
    {
        public string Search { get; set; }

        public int? SelectedSubjectId { get; set; }
        public int? SelectedClassId { get; set; }

        public List<Subject> Subjects { get; set; }
        public List<Classes> Classes { get; set; }

        // هنا البيانات اللي هتتعرض في التقرير
        public List<CourseReportVM> Courses { get; set; } = new List<CourseReportVM>();
    }
}
