using Luno_platform.Models;
using Luno_platform.Viewmodel;

namespace Luno_platform.Service
{
    public interface IstudentService : I_BaseService<Student>
    {
        Student GetStudent(int id);
        List<Courses> GetStudentCourses(int id);
        List<Courses> GetStudentCourses(int studentId, int page = 1, int pageSize = 10);
        List<StudentCourseFullDataVM> GetStudentCoursesFullData(int id);
         List<Payments> GetPayments(int studentId);
        int? GetStudentIdByUserId(int userId);
        int getStudentId(int userid);
        public bool ChangeStudentPassword(int studentId, string oldPassword, string newPassword);
        bool isSubdcrip(int studentid, int courseid);
        public List<showallStudent> showStudents();
        void DeleteStudent(int userid);
        void SetUserPending(int userid);

    }
}
