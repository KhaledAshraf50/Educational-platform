using Luno_platform.Models;
using Luno_platform.Viewmodel;

namespace Luno_platform.Repository
{
    public interface IstudentRepo: I_BaseRepository<Student>
    {
        Student GetStudent(int id);
        List<Courses> GetStudentCourses(int id);
        List<StudentCourseFullDataVM> GetStudentCoursesFullData(int id);

    }
}
