using Luno_platform.Service;
using Microsoft.AspNetCore.Mvc;

namespace Luno_platform.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        public IActionResult Dashboard()
        {
            // جلب الـ UserId من الـ Claims (أو من الـ session حسب تطبيقك)
            var userId = 2;
            //var userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);

            var model = _adminService.GetDashboardData(userId);

            return View(model);
        }
        [Route("/Admin/mainpage")]
        public IActionResult mainpage()
        {
            // جلب الـ UserId من الـ Claims (أو من الـ session حسب تطبيقك)
            var userId = 2;
            //var userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);

            var model = _adminService.GetDashboardData(userId);

            return View(model);
        }
        public IActionResult users()
        {
            return View();
        }
        public IActionResult courses()
        {
            return View();
        }
        public IActionResult payments()
        {
            return View();
        }
        public IActionResult Report()
        {
            return View();
        }
        public IActionResult Main()
        {
            return View();
        }
        public IActionResult setting()
        {
            return View();
        }
    }
}
