namespace Luno_platform.Models
{
    public class InstructorDashboardViewModel
    {
        
            public Instructor Instructor { get; set; }
            public List<Courses> Courses { get; set; }
            public List<Classes> Classes { get; set; }
            public int TotalClasses { get; set; }
            public int TotalSales { get; set; }
            public int AvailableCourses { get; set; }
        
    }
}
