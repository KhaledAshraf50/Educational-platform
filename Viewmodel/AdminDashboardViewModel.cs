namespace Luno_platform.Viewmodel
{
    public class AdminDashboardViewModel
    {
        public string AdminName { get; set; }
        public string AdminImage { get; set; }
        public int TotalStudents { get; set; }
        public int TotalInstructors { get; set; }
        public int TotalCoursesAccepted { get; set; }
        public int TotalCoursesPending { get; set; }
        public decimal TotalPayments { get; set; }
    }
}
