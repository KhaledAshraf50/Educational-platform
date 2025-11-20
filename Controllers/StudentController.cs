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
            var model  = new mainPage_Student_ViewModel() {
                Student = istudentService.GetStudent(id),
                Courses = istudentService.GetStudentCourses(id)
            };

            
            
            return View(model);
        }
        
        public IActionResult ReportsPage()
        {
            return View("ReportsPage");
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
