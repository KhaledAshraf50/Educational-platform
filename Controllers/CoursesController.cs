using Luno_platform.Service;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Mvc;

namespace Luno_platform.Controllers
{
    public class CoursesController : Controller
    {
        private readonly courses_service _courses;

        public CoursesController(courses_service courses)
        {
            _courses = courses;
        }

        public IActionResult Index( )
        {
            return View();
        }

        [HttpGet]
        public IActionResult filter()
        {


            var model = new filter_coursesviewmodel
            {

                Courses = _courses.showallcourses()
            };


            return View(model);
        }

        [HttpPost]
        public IActionResult filter(filter_coursesviewmodel filter)
        {
            return View();
        }



    }
}
