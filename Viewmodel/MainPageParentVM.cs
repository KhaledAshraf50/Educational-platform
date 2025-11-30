using Luno_platform.Models;

namespace Luno_platform.Viewmodel
{
    public class MainPageParentVM
    {
        public List<Student>? Student { get; set; }
        public Parent parent { get; set; }
        public List<Courses>? Courses { get; set; }
        public Dictionary<int, double> ExamProgressDict { get; set; }
        public Dictionary<int, double> TaskProgressDict { get; set; }
        public Dictionary<int, double> OverallProgressDict { get; set; }
    }
}
