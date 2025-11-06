using Microsoft.AspNetCore.Mvc;

namespace Luno.Controllers
{
    public class StudentController : Controller
    {
        [Route("/Student/MainPage")]
        public IActionResult MainPage()
        {
            return View("MainPage");
        }
        [Route("/Student/ReportsPage")]
        public IActionResult ReportsPage()
        {
            return View("ReportsPage");
        }
    }
}
