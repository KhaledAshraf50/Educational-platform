using Luno_platform.Models;

namespace Luno_platform.Viewmodel
{
    public class DashboardVM
    {
        public List<AssignmentVM> Assignments { get; set; }
        public List<ExamVM> Exams { get; set; }

        // Stats
        public int TotalAssignments => Assignments?.Count ?? 0;
        public int TotalExams => Exams?.Count ?? 0;
        public int StudentsSubmitted => Assignments?.Sum(a => a.StudentsSubmitted) ?? 0;
        public int StudentsTotalAssignments => Assignments?.Sum(a => a.TotalStudents) ?? 0;
        public int StudentsTakenExams => Exams?.Sum(e => e.StudentsTaken) ?? 0;
        public int StudentsTotalExams => Exams?.Sum(e => e.TotalStudents) ?? 0;
    }
    public class AssignmentVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public int TotalQuestions { get; set; }
        public int StudentsSubmitted { get; set; }
        public int TotalStudents { get; set; }
    }

    public class ExamVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public int TotalQuestions { get; set; }
        public int Duration { get; set; } // بالدقائق
        public int StudentsTaken { get; set; }
        public int TotalStudents { get; set; }
    }

}
