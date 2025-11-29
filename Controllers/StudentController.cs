using Luno_platform.Models;
using Luno_platform.Repository;
using Luno_platform.Service;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luno.Controllers
{

    [Authorize(Roles = "student")]
    public class StudentController : Controller
    {
       public IstudentService istudentService;

        public IParentRepo parentRepo;
        public StudentController(IstudentService studentService, IParentRepo parentRepo)
        {
            istudentService = studentService;
            this.parentRepo = parentRepo;
        }
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
            // جلب الـ UserId من الـ Claims اللي اتخزنت مع تسجيل الدخول
            //var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            //if (userIdClaim == null)
            //{
            //    return Unauthorized();
            //}

            //int userId = int.Parse(userIdClaim.Value); // ده Id اليوزر المتسجل فقط
            int userId = GetUserId()
            ; // ده Id اليوزر المتسجل فقط

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
        [Route("/Student/ReportsPage")]
        public IActionResult ReportsPage()
        {
            int userId = GetUserId();
            List<StudentCourseFullDataVM> courses = istudentService.GetStudentCoursesFullData(userId);
            return View(courses);
        }
        [Route("/Student/SubjectsPage")]
        public IActionResult SubjectsPage( int page = 1)

        {
            int userId = GetUserId();
            int pageSize = 10;
            var courses = istudentService.GetStudentCourses(userId, page, pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.StudentId = userId;
            return View(courses);
        }

        [Route("/Student/invoicesPage")]
        public IActionResult invoicesPage()
        {
            int userId = GetUserId();
            List<Payments> payments = istudentService.GetPayments(userId); // استبدل 1 بالمعرف الصحيح للطالب
            return View(payments);
        }
        public IActionResult SettingPage()
        {
            int userId = GetUserId();
            return View("SettingPage");
        }
    }
}