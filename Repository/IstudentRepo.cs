using Luno_platform.Models;

namespace Luno_platform.Repository
{
    public interface IstudentRepo: I_BaseRepository<Student>
    {
        Student GetStudent(int id);
        List<Courses> GetStudentCourses(int id);
    }
}
