
using Luno_platform.Models;

namespace Luno_platform.Viewmodel
{
    public class mainPage_Student_ViewModel
    {
        public Student? Student { get;  set; }
        public List<Courses> Courses { get;  set; }
        public double OverallProgress { get; set; }
        public double ExamProgress { get; set; }
        public double TaskProgress { get; set; }


    }
}
