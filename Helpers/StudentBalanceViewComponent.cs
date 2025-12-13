using Luno_platform.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Luno_platform.Helpers
{
    public class StudentBalanceViewComponent : ViewComponent
    {
        private readonly IstudentService _studentService;

        public StudentBalanceViewComponent(IstudentService studentService)
        {
            _studentService = studentService;
        }
        public IViewComponentResult Invoke()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("student")) return Content("");
            var userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var student = _studentService.GetStudent(userId);
            return View(student?.Balance ?? 0);
        }
    }
}
