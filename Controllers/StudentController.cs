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
            // جلب الـ UserId من الـ Claims اللي اتخزنت مع تسجيل الدخول
            //var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            //if (userIdClaim == null)
            //{
            //    return Unauthorized();
            //}

            //int userId = int.Parse(userIdClaim.Value); // ده Id اليوزر المتسجل فقط
            int userId = GetUserId()
            ; // ده Id اليوزر المتسجل فقط

            // نجيب بيانات الطالب اللي مربوط باليوزر ده
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
            //int pageSize = 10;
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
            // استبدل 1 بالمعرف الصحيح للطالب
            //int pageSize = 10;


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

            return RedirectToAction("invoicesPage"); // 🔥 أهم نقطة هنا
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
        //[HttpPost]
        //public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        //{
        //    if (model.NewPassword != model.ConfirmPassword)
        //    {
        //        ModelState.AddModelError("", "كلمة السر الجديدة غير متطابقة");
        //        return View(model);
        //    }

        //    var userId = GetUserId();
        //    var user = await _userManager.FindByIdAsync(userId.ToString());

        //    var result = await _userManager.ChangePasswordAsync(
        //        user,
        //        model.CurrentPassword,
        //        model.NewPassword
        //    );

        //    if (!result.Succeeded)
        //    {
        //        foreach (var error in result.Errors)
        //            ModelState.AddModelError("", error.Description);

        //        return View(model);
        //    }

        //    ModelState.AddModelError("", "كلمة السر الجديدة غير متطابقة");
        //    var settings = _service.GetSettings(userId);
        //    settings.PasswordModel = model;
        //    return View("SettingPage", settings);
        //    // يرجع لصفحة الإعدادات
        //}
        public IActionResult ChangePassword(UserSettingsVM SVM)
        {
            //int userId = GetUserId();
            //var student = istudentService.GetStudent(userId);
            //if (student == null) return NotFound();
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