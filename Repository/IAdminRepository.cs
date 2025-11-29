using Luno_platform.Models;
using System.Collections.Generic;

namespace Luno_platform.Repository
{
    public interface IAdminRepository
    {
        Users GetAdminByUserId(int userId);
        int GetTotalStudents();
        int GetTotalInstructors();
        int GetTotalCoursesByStatus(string status);
        int GetTotalCoursesPending();
        decimal GetTotalPayments();
    }
}
