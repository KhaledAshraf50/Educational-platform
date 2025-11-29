using Luno_platform.Service;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Mvc;

namespace Luno_platform.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITask_service _TaskService;

        public IActionResult Index()
        {
            return View();
        }

        public TaskController(ITask_service TaskService)
        {
            _TaskService = TaskService;
        }

        [HttpPost]
        public IActionResult SubmitExam(SubmitTaskModel model)
        {
            int studentId = 1; // هتجيبها من السيشن عندك

            int degree = _TaskService.CorrectTaskAndSave(model.TaskID, studentId, model.Answers);

            TempData["Degree"] = degree;

            return RedirectToAction("TaskSubmitted");
        }

        public IActionResult TaskSubmitted()
        {
            TempData["Message"] = "Task submitted successfully!";
            return View();
        }
    }
}
