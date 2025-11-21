using Luno_platform.Service;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Mvc;

namespace Luno_platform.Controllers
{
    public class HomepageController : Controller
    {
        public readonly I_homepage_serves _homeservice;
        public readonly I_instructor_services _i_Instructor_Services;

        public HomepageController(I_homepage_serves homeservice , I_instructor_services i_Instructor_Services )
        {
         
            _homeservice = homeservice;

            _i_Instructor_Services = i_Instructor_Services;


        }

        public IActionResult mainpage()
        {
            var model = new homepage_viewmodel
            {
                subjects = _homeservice.GetSubjects,
                instructor=_i_Instructor_Services.infoinstructors()
           

            };
            return View(model);
        }

        

        public IActionResult login_in()
        {
            return View();
        }


        public IActionResult sign_up()
        {
            return View();
        }

        public IActionResult show_all_teacher()
        {
            var instructor = _i_Instructor_Services.GetAll_Instructors_With_User_with_subject();

            return View(instructor);
        }

        public IActionResult show_portfolio_teacher(int id )
        {
            var teacher= _i_Instructor_Services.getprotfolioteacher(id);
            return View(teacher);
        }

        public IActionResult show_courses_teacher()
        {
            return View();
        }


        public IActionResult show_details_courses()
        {
            return View();
        }



    }


}
