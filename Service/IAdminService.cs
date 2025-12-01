
using Luno_platform.Models;
using Luno_platform.Viewmodel;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Service
{
    public interface IAdminService:I_BaseService<Admin>
    {
        AdminDashboardViewModel GetDashboardData(int userId);
        public AdminSettingVM GetAdminSetting(int id);
        public bool UpdateAdminSetting(AdminSettingVM AVM);
        public void UpdateImage(int adminId, string imgUrl);
        public Users GetAdminByUserId(int userId);
        public Admin GetAdmin(int userId);
        public bool ChangeAdminPassword(int adminId, string oldPassword, string newPassword);
    }
}
