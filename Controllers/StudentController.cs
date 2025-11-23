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
        [Route("/Student/MainPage/{id}")]
        public IActionResult MainPage(int id)
        {
            var student = istudentService.GetStudent(id);
            if (student == null)
            {
                return NotFound("الطالب غير موجود");
            }

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
        
        public IActionResult SubjectsPage()
        {
            return View("SubjectsPage");
        }
        public IActionResult invoicesPage()
        {
            return View("invoicesPage");
        }
        public IActionResult SettingPage()
        {
            return View("SettingPage");
        }
    }
}
