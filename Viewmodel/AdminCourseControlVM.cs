using Luno_platform.Models;

namespace Luno_platform.Viewmodel
{
    public class AdminCourseControlVM
    {
        public List<CourseInfoVM> active_course { get; set; }
        public List<CourseInfoVM> pending_course { get; set; }
    }


}
