using Luno_platform.Models;

namespace Luno_platform.Service
{
    public interface IstudentService: I_BaseService<Student>
    {
        Student GetStudent(int id);
        List<Courses> GetStudentCourses(int id);
    }
}
