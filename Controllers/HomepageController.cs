using Luno_platform.Models;
using Luno_platform.Service;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Mvc;

namespace Luno_platform.Controllers
{
    public class HomepageController : Controller
    {
        public readonly I_homepage_serves _homeservice;
        public readonly I_instructor_services _i_Instructor_Services;
        public readonly Icourses_service _icourses_Service;
        public readonly  IExam_service _exam_Service;
        public readonly ITask_service _Task_Service;

        public readonly I_BaseService<Users> baseService;



        public HomepageController(I_homepage_serves homeservice , I_instructor_services i_Instructor_Services , Icourses_service icourses_Service, IExam_service exam_Service)
        {
         
            _homeservice = homeservice;

            _i_Instructor_Services = i_Instructor_Services;

            _icourses_Service = icourses_Service;
            _exam_Service = exam_Service;





        }

        public int getuserid()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return -1;
            }

            int userId = int.Parse(userIdClaim.Value);
            return userId;
        }
        public IActionResult mainpage()
        {
            var userid = getuserid();
            var model = new homepage_viewmodel
            {
                subjects = _homeservice.GetSubjects,
                instructor = _i_Instructor_Services.infoinstructors()
              
               


            };

            return View(model);
        }

        //public IActionResult imageportfolio()
        //{
        //    var userid = getuserid();

        //    var image = baseService.GetImagePortfolio(userid);

        //    ViewBag.image = image;

          

        //    return View("~/Views/Shared/_navbar.cshtml");
        //}



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

        [Route("homepage/show_courses_teacher/{structorid}/{classId}")]
        public IActionResult show_courses_teacher(int structorid,int classid)
        {
            var courses = _icourses_Service.showAllcoursebyclassandinstructor(structorid, classid);

            return View(courses);
        }

        [Route("Homepage/show_details_courses/{courseid}")]
        public IActionResult show_details_courses(int courseid)
        {
            var course = _icourses_Service.Infocourse(courseid);
            if (course == null) return NotFound();
            return View(course);
        }


        [Route("homepage/pageExam/{Examid}")]
        public IActionResult pageExam(int Examid)
        {
            var model = new pageExam_viewmodel { 
            
                questions = _exam_Service.GetExamsbyid(Examid),
                examinfo = _exam_Service.GetById(Examid)
            };

            

            return View(model);
        }


        [Route("homepage/pageTask/{Taskid}")]
        public IActionResult pageTask(int Taskid)
        {
            var model = new pageTask_viewmodel
            {

                questions = _Task_Service.GetTaskbyid(Taskid),
                Taskinfo= _Task_Service.GetById(Taskid)
            };



            return View(model);
        }



    }


}
