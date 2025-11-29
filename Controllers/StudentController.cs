using Luno_platform.Models;
using Luno_platform.Repository;
using Luno_platform.Service;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luno.Controllers
{
    public class StudentController : Controller
    {
        public IstudentService istudentService;
        public IParentRepo parentRepo;
        public StudentController(IstudentService studentService, IParentRepo parentRepo)
        {
            istudentService = studentService;
            this.parentRepo = parentRepo;
        }
        [Authorize(Roles = "student")]
        public int GetUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return -1; // معناها مفيش يوزر
            }

            return int.Parse(userIdClaim.Value);
        }
        [Route("/Student/MainPage")]
        public IActionResult MainPage()
        {
            int userId = GetUserId(); // ده Id اليوزر المتسجل فقط

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
