using Luno_platform.Models;
using Luno_platform.Viewmodel;

namespace Luno_platform.Service
{
    public interface IstudentService: I_BaseService<Student>
    {
        Student GetStudent(int id);
        List<Courses> GetStudentCourses(int id);
        List<StudentCourseFullDataVM> GetStudentCoursesFullData(int id);
    }
}
