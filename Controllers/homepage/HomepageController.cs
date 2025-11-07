using Microsoft.AspNetCore.Mvc;

namespace Luno_platform.Controllers.homepage
{
  
    public class HomepageController : Controller
    {
        public IActionResult page()
        {
            return View();
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
            return View();
        }

        public IActionResult show_portfolio_teacher()
        {
            return View();
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
