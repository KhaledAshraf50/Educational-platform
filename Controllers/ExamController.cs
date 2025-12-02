using Luno_platform.Service;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luno_platform.Controllers
{

    [Authorize(Roles = "student")]

    public class ExamController : Controller
    {
        private readonly IExam_service _examService;
        private readonly IstudentService _istudentService;

        public ExamController(IExam_service exam_Service , IstudentService istudentService)
        {
            _examService = exam_Service;

            _istudentService = istudentService;



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

        public IActionResult Index()
        {
            return View();
        }


      

        [HttpPost]
        public IActionResult SubmitExam(SubmitExamModel model)
        {
            var userid = getuserid();
            var studentid = _istudentService.getStudentId(userid);

            int degree = _examService.CorrectExamAndSave(model.ExamID, studentid, model.Answers);

            TempData["Degree"] = degree;

            return RedirectToAction("homepage", "pageExam");
        }
        
        public IActionResult ExamSubmitted()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SubmitTask(SubmitTaskModel model)
        {
            var userid = getuserid();
            var studentid = _istudentService.getStudentId(userid);

            int degree = _examService.CorrectTaskAndSave(model.TaskID, studentid, model.Answers);

            // بدل Redirect لصفحة ExamSubmitted، نروح لصفحة DetailsCourses
            TempData["Popup"] = $"تم تصحيح الامتحان! درجتك: {degree}";

            // هنا نفترض أن الـ CourseId موجود عندك في الـ model أو تحضره من الخدمة
            return RedirectToAction("Details", "Courses");
        }

        [HttpGet]
        public IActionResult SubmitTask()
        {
            return View();
        }

    }
}
