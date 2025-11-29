using Luno_platform.Service;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Mvc;

namespace Luno_platform.Controllers
{
    public class ExamController : Controller
    {
        private readonly IExam_service _examService;

        public IActionResult Index()
        {
            return View();
        }


        public ExamController(IExam_service examService)
        {
            _examService = examService;
        }

        [HttpPost]
        public IActionResult SubmitExam(SubmitExamModel model)
        {
            int studentId = 3; // هتجيبها من السيشن عندك

            int degree = _examService.CorrectExamAndSave(model.ExamID, studentId, model.Answers);

            TempData["Degree"] = degree;

            return RedirectToAction("ExamSubmitted");
        }

        public IActionResult ExamSubmitted()
        {
            return View();
        }
    }
}
