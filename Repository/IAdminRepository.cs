using Luno_platform.Models;
using Luno_platform.Viewmodel;
using System.Collections.Generic;

namespace Luno_platform.Repository
{
    public interface IAdminRepository:I_BaseRepository<Admin>
    {
        Users GetAdminByUserId(int userId);
        int GetTotalStudents();
        int GetTotalInstructors();
        int GetTotalCoursesByStatus(string status);
        int GetTotalCoursesPending();
        decimal GetTotalPayments();
        public Admin GetAdmin(int userId);
        public List<Courses> GetallActiveCourses();
        public List<Courses> GetallpendingCourses();
        public AdminCourseControlVM GetCourseControl();
        public void ChangeCourseStatus(int courseId, string newStatus);
        public void DeleteCourse(int courseId);
    }
}
