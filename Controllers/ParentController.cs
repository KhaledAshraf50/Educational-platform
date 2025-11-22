using Microsoft.AspNetCore.Mvc;

namespace Luno_platform.Controllers
{
    public class ParentController : Controller
    {
        public IActionResult MainPage()
        {
            return View();
        }
        public IActionResult Childerns()
        {
            return View();
        }

        public IActionResult Invoices()
        {
            return View();
        }
        public IActionResult Settings()
        {
            return View();
        }
    }
}
