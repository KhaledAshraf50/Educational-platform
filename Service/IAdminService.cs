
using Luno_platform.Viewmodel;

namespace Luno_platform.Service
{
    public interface IAdminService
    {
        AdminDashboardViewModel GetDashboardData(int userId);
        public AdminSettingVM GetAdminSetting(int id);
        public bool UpdateAdminSetting(AdminSettingVM AVM);
        public void UpdateImage(int adminId, string imgUrl);
    }
}
