using Luno_platform.Models;
using Luno_platform.Service;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luno.Controllers
{
   

    public class StudentController : Controller
    {
       public IstudentService istudentService;
        public StudentController(IstudentService studentService)
        {
            istudentService = studentService;
        }
        [Authorize(Roles = "student")]
        [Route("/Student/MainPage")]
        public IActionResult MainPage()
        {
            // جلب الـ UserId من الـ Claims اللي اتخزنت مع تسجيل الدخول
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdClaim.Value); // ده Id اليوزر المتسجل فقط

            // نجيب بيانات الطالب اللي مربوط باليوزر ده
            var student = istudentService.GetStudent(userId);
            if (student == null)
            {
                return NotFound("الطالب غير موجود");
            }

            var vm = new mainPage_Student_ViewModel
            {
                Student = student,
                Courses = istudentService.GetStudentCourses(userId)
            };

            return View(vm);
        }
        [Route("/Student/ReportsPage/{id}")]
        public IActionResult ReportsPage(int id)
        {
            List<StudentCourseFullDataVM> courses = istudentService.GetStudentCoursesFullData(id);
            return View(courses);
        }
        [Route("/Student/SubjectsPage/{id}")]
        public IActionResult SubjectsPage(int id, int page = 1)
        {
            int pageSize = 10;
            var courses = istudentService.GetStudentCourses(id, page, pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.StudentId = id;
            return View(courses);
        }

        [Route("/Student/invoicesPage/{id}")]
        public IActionResult invoicesPage(int id)
        {
            List<Payments> payments = istudentService.GetPayments(id); // استبدل 1 بالمعرف الصحيح للطالب
            return View(payments);
        }
        public IActionResult SettingPage()
        {
            return View("SettingPage");
        }
    }
}
