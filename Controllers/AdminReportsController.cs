using Luno_platform.Service;
using Microsoft.AspNetCore.Mvc;

namespace Luno_platform.Controllers
{
    public class AdminReportsController : Controller
    {
        private readonly IReportsService _reportsService;

        public AdminReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        public IActionResult Report(string filter = "thisMonth")
        {
            var model = _reportsService.GetReport(filter);
            return View(model);
        }
    }
}
