
using Luno_platform.Viewmodel;

namespace Luno_platform.Service
{
    public interface IAdminService
    {
        AdminDashboardViewModel GetDashboardData(int userId);
    }
}
