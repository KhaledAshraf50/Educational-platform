using Microsoft.AspNetCore.Mvc;

namespace Luno_platform.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult users()
        {
            return View();
        }

    }
}
