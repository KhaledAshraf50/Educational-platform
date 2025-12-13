using Luno_platform.Models;
using Luno_platform.Repository;
using Luno_platform.Service;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Luno.Controllers
{

    [Authorize(Roles = "student")]
    public class StudentController : Controller
    {
       private IstudentService istudentService;
        private readonly SettingsService _service;
        private readonly UserManager<Users> _userManager;

        public IParentRepo parentRepo;
        public StudentController(IstudentService studentService, IParentRepo parentRepo ,SettingsService service, UserManager<Users> userManager)
        {
            istudentService = studentService;
            this.parentRepo = parentRepo;
            _service = service;
            _userManager = userManager;
        }
        public int GetUserId()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return -1; // معناها مفيش يوزر
            }

            return int.Parse(userIdClaim.Value);
        }

        [Route("/Student/MainPage")]
        public IActionResult MainPage()
        {
            
            int userId = GetUserId();
            
            var student = istudentService.GetStudent(userId);
            if (student == null)
            {
                return NotFound("الطالب غير موجود");
            }
            var progress = parentRepo.GetStudentProgress(student.StudentID);

            var vm = new mainPage_Student_ViewModel
            {
                Student = student,
                Courses = istudentService.GetStudentCourses(userId),
                OverallProgress = progress.OverallProgress,
                ExamProgress = progress.ExamProgress,
                TaskProgress = progress.TaskProgress
            };

            return View(vm);
        }
        [Route("/Student/ReportsPage")]
        public IActionResult ReportsPage()
        {
            int userId = GetUserId();
            List<StudentCourseFullDataVM> courses = istudentService.GetStudentCoursesFullData(userId);
            return View(courses);
        }
        [Route("/Student/SubjectsPage")]
        public IActionResult SubjectsPage( int page = 1)
        {
            int pageSize = 7;

            int userId = GetUserId();
           
            var courses = istudentService.GetStudentCourses(userId);


            var pagedStudents = courses
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            int totalPages = (int)Math.Ceiling(courses.Count / (double)pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            return View(pagedStudents);
        }

        [Route("/Student/invoicesPage")]
        public IActionResult invoicesPage(int page = 1)
        {
            int pageSize = 7;
            int userId = GetUserId();
            List<Payments> payments = istudentService.GetPayments(userId);
           


            var pagedStudents = payments
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            int totalPages = (int)Math.Ceiling(payments.Count / (double)pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            var student = istudentService.GetStudent(userId);
            ViewBag.Balance = student.Balance;

            return View(pagedStudents);
        }

        [HttpPost]
        public IActionResult chargeBalance(decimal amount)
        {
            int userId = GetUserId();


            istudentService.ChargeBalance(userId, amount);





            TempData["msg"] = "تم شحن الرصيد بنجاح ✔️";

            return RedirectToAction("invoicesPage"); 
        }


        public IActionResult SettingPage()
        {
            int userId = GetUserId();
            var model = _service.GetSettings(userId);
            return View(model);
        }
        [HttpPost]
        public IActionResult Save(UserSettingsVM model)
        {
            _service.UpdateSettings(model);
            return RedirectToAction("SettingPage");
        }
       
        public IActionResult ChangePassword(UserSettingsVM SVM)
        {   
            
            bool ok = istudentService.ChangeStudentPassword(SVM.UserId, SVM.CurrentPassword, SVM.ConfirmNewPassword);
            if (!ok)
            {
                TempData["Error"] = "كلمه المرور غير صحيحة!!";
                return RedirectToAction("SettingPage");
            }

            TempData["Sucess"] = "تم تغيير كلمه المرور بنجاح";
            return RedirectToAction("SettingPage");
        }

    }
}