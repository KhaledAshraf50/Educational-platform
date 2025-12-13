namespace Luno_platform.Models
{
    public class InstructorDashboardViewModel
    {
        
            public Instructor Instructor { get; set; }
            public List<Courses> Courses { get; set; }
            public List<Classes> Classes { get; set; }
        public List<Classes> AllClasses { get; set; } // ⬅️ كل الصفوف (للدروب داون)

        public int SelectedClassID { get; set; } // ⬅️ الصف المختار
        public Classes Classe { get; set; }
            public int TotalClasses { get; set; }
            public int TotalSales { get; set; }
        public int AvailableCourses { get; set; }
        public int InstructorID { get; set; }

    }
}
