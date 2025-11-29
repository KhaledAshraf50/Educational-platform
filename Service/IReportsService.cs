using Luno_platform.Models;
using Luno_platform.Viewmodel;

namespace Luno_platform.Service
{
    public interface IReportsService 
    {
        AdminReportsViewModel GetReport(string filter);
    }
}
