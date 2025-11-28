using Luno_platform.Models;
using Luno_platform.Viewmodel;

namespace Luno_platform.Repository
{
    public interface IstudentRepo : I_BaseRepository<Student>
    {
        Student GetStudent(int id);
        List<Courses> GetStudentCourses(int id);
        List<Courses> GetStudentCourses(int studentId, int page = 1, int pageSize = 10);
        List<StudentCourseFullDataVM> GetStudentCoursesFullData(int id);
<<<<<<< HEAD
         List<Payments> GetPayments(int studentId);
=======
<<<<<<< HEAD
         List<Payments> GetPayments(int studentId);
=======
        List<Payments> GetPayments(int studentId);
>>>>>>> origin/back_homepage
>>>>>>> dev
        int? GetStudentIdByUserId(int userId);

    }
}