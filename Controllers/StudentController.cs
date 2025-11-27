using Luno_platform.Models;
using Luno_platform.Service;
using Luno_platform.Viewmodel;
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
        [Route("Student/MainPage/{id:int}")]
        public IActionResult MainPage(int id)
        {
            var student = istudentService.GetStudent(id);
           
            var vm = new mainPage_Student_ViewModel
            {
                Student = student,
                Courses = istudentService.GetStudentCourses(id)
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
