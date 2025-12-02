using Luno_platform.Models;
using Luno_platform.Repository;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Identity;

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
        public AdminSettingVM GetAdminSetting(int id)
        {
            var admin = _adminRepo.GetAdmin(id);
            AdminSettingVM AVM = new AdminSettingVM()
            {
                AdminId = admin.User.Id,
                Image = admin.User.Image,
                Name = admin.User.fname + " " + admin.User.lastName,
                Email = admin.User.Email,
                PhoneNumber = admin.User.PhoneNumber,
                NationalId = admin.User.nationalID
            };
            return AVM;
        }

        public bool UpdateAdminSetting(AdminSettingVM AVM)
        {
            var admin = _adminRepo.GetAdmin(AVM.AdminId);
            if (admin == null)
                return false;
            admin.User.fname = AVM.Name.Split(' ')[0];
            admin.User.lastName = AVM.Name.Split(' ').Length > 1 ? AVM.Name.Split(' ')[1] : "";
            admin.User.Email = AVM.Email;
            admin.User.PhoneNumber = AVM.PhoneNumber;
            admin.User.nationalID = AVM.NationalId;
            _adminRepo.Update(admin);
            _adminRepo.Save();
            return true;
        }
        public void UpdateImage(int adminId, string imgUrl)
        {
            var admin = _adminRepo.GetAdmin(adminId);

            if (admin == null || admin.User == null)
                throw new Exception("Parent not found");

            admin.User.Image = imgUrl;

            _adminRepo.Update(admin);
            _adminRepo.Save();
        }

        public List<Courses> GetallActiveCourses()
        {
            return _adminRepo.GetallActiveCourses();
        }

        public List<Courses> GetallpendingCourses()
        {
            return _adminRepo.GetallpendingCourses();
        }

        public AdminCourseControlVM GetCourseControl()
        {
            return _adminRepo.GetCourseControl();
        }
        public void ChangeCourseStatus(int courseId, string newStatus)
        {
             _adminRepo.ChangeCourseStatus(courseId, newStatus);
        }
        public void DeleteCourse(int courseId)
        {
            _adminRepo.DeleteCourse(courseId);
        }

        //public bool ChangeParentPassword(int parentId, string oldPassword, string newPassword)
        //{
        //    var parent = parentRepo.GetParent(parentId);
        //    if (parent == null) return false;
        //    var hasher = new PasswordHasher<Users>();
        //    var result = hasher.VerifyHashedPassword(parent.User, parent.User.PasswordHash, oldPassword);
        //    if (result == PasswordVerificationResult.Failed)
        //    {
        //        return false;
        //    }
        //    parent.User.PasswordHash = newPassword;
        //    _adminRepo.Update(parent);
        //    _adminRepo.Save();
        //    return true;
        //}

    }
}
