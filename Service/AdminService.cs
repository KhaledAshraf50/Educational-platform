using Luno_platform.Viewmodel;
using Luno_platform.Repository;

namespace Luno_platform.Service
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepo;

        public AdminService(IAdminRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }

        public AdminDashboardViewModel GetDashboardData(int userId)
        {
            var admin = _adminRepo.GetAdminByUserId(userId);

            return new AdminDashboardViewModel
            {
                AdminName = admin.fname + " " + admin.lastName,
                AdminImage = admin.Image,
                TotalStudents = _adminRepo.GetTotalStudents(),
                TotalInstructors = _adminRepo.GetTotalInstructors(),
                TotalCoursesAccepted = _adminRepo.GetTotalCoursesByStatus("مقبول"),
                TotalCoursesPending = _adminRepo.GetTotalCoursesPending(),
                TotalPayments = _adminRepo.GetTotalPayments()
            };
        }
    }
}
