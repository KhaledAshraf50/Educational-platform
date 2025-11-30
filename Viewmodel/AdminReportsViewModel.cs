namespace Luno_platform.Viewmodel
{
    public class AdminReportsViewModel
    {
      
            public int TotalCoursesSold { get; set; }
            public decimal TotalRevenue { get; set; }
            public decimal ProfitRate { get; set; }

            public List<TopInstructorVM> TopInstructors { get; set; }
            public List<TopCourseVM> TopCourses { get; set; }
            public string SelectedFilter { get; set; }
        }

        public class TopInstructorVM
        {
            public string InstructorName { get; set; }
            public int TotalSold { get; set; }
            public decimal Revenue { get; set; }
        }

        public class TopCourseVM
        {
            public string CourseName { get; set; }
            public int TotalSold { get; set; }
            public decimal Revenue { get; set; }
        }

    }

