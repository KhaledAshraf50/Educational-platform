using Luno_platform.Helpers;
using Luno_platform.Models;
using Luno_platform.Repository;
using Luno_platform.Service;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Luno_platform.Controllers
{
    [Authorize(Roles ="parent")]
    public class ParentController : Controller
    {
        IParentService _parentService;
        IParentRepo _parentRepo;

        public ParentController(IParentService parentService, IParentRepo parentRepo)
        {
            _parentService = parentService;
            _parentRepo = parentRepo;
        }
        public int GetUserId()
        {

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return -1;
            }
            return int.Parse(userIdClaim.Value);
        }
        public IActionResult MainPage()
        {
            int userId = GetUserId();
            var parent = _parentRepo.GetByUserId(userId);
            if (parent == null) return NotFound();

            int parentId = parent.ID;

            var parentData = _parentService.GetParent(parentId);
            var students = _parentService.GetStds(parentId);
            var vm = new MainPageParentVM
            {
                Student = students,
                parent = parentData,
                Courses = _parentService.GetStudentCourses(parentId),
                ExamProgressDict = new Dictionary<int, double>(),
                TaskProgressDict = new Dictionary<int, double>(),
                OverallProgressDict = new Dictionary<int, double>()
            };
            foreach (var std in students)
            {
                var progress = _parentRepo.GetStudentProgress(std.StudentID);
                vm.ExamProgressDict[std.StudentID] = progress.ExamProgress;
                vm.TaskProgressDict[std.StudentID] = progress.TaskProgress;
                vm.OverallProgressDict[std.StudentID] = progress.OverallProgress;
        }
            return View(vm);
        }
        //------------------------
        public IActionResult Childerns()
        {
            int userId = GetUserId();
            var parent = _parentRepo.GetByUserId(userId);
            if (parent == null) return NotFound();

            var vm = new ParentChildrenViewModel
            {
                ParentID = parent.ID,
                Students = _parentService.GetStds(parent.ID)
            };

            return View(vm);
        }

        public IActionResult ChildDetails(int id)
        {
            if (id <= 0) return RedirectToAction("Childerns");

            Student std = _parentService.GetStudentDetails(id);
            if (std == null) return RedirectToAction("Childerns");

            return View(std);
        }

        public IActionResult AddChild()
        {
            int userId = GetUserId();
            var parent = _parentRepo.GetByUserId(userId);

            ChildViewModel vm = new ChildViewModel
            {
                ParentID = parent.ID
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult SaveAddChild(ChildViewModel childvm)
        {
            if(!ModelState.IsValid)
            {
                return View("AddChild", childvm);
            }
            var student = _parentService.FindChild(childvm.NationalID);
            if(student == null)
            {
                ModelState.AddModelError("NationalID", "A student with this National ID not Exist.");
                return View("AddChild", childvm);
            }
            var vm = new ConfirmChildViewModel
            {
                StudentID = student.StudentID,
                FullName = student.User.fname+" "+student.User.lastName,
                NationalID = student.User.nationalID,
                ParentID = childvm.ParentID,
                ClassName= student.Classes.ClassName
            };
            return View("ConfirmChild", vm);
        }
        [HttpPost]
        public IActionResult ConfirmAddChild(int parentID,int studentID)
        {
            
           var success = _parentService.LinkChild(parentID, studentID,out string error);
            if (!success)
            {
                TempData["Error"] = error;
                return RedirectToAction("AddChild");
            }
            return RedirectToAction("Childerns", new { id = parentID });
        }

        //------------------------

        public IActionResult Invoices()
        {
            int userId = GetUserId();
            var parent = _parentRepo.GetByUserId(userId);
            var students = _parentRepo.GetStudentBelongToParent(parent.ID);
            List<Payments> allPayments = new List<Payments>();

            foreach (var student in students)
            {
                int studentId = student.StudentID;
                var payments = _parentRepo.GetPayments(studentId);

                allPayments.AddRange(payments);
            }

            var totalPaid = allPayments
             .Where(p => p.status == "مقبول")
             .Sum(p => p.amountPayment);

            // إجمالي الغير مدفوع (مرفوض أو قيد المراجعة)
            var totalUnpaid = allPayments
                .Where(p => p.status == "مرفوض" || p.status == "قيد المراجعة")
                .Sum(p => p.amountPayment);

            ViewBag.TotalPaid = totalPaid;
            ViewBag.TotalUnpaid = totalUnpaid;
            return View(allPayments);  
        }
        //------------------------
        public IActionResult Settings()
        {
            int userId = GetUserId();
            var parent = _parentRepo.GetByUserId(userId);

            var vm = _parentService.GetParentSetting(parent.ID);
            return View(vm);
        }
   
        [HttpPost]
        public IActionResult UploadImage(IFormFile file)
        {
            int userId = GetUserId();
            var parent = _parentRepo.GetByUserId(userId);
            if (parent == null) return NotFound();

            int parentID = parent.ID;

            if (file == null || file.Length == 0)
            {
                TempData["ErrorFile"] = "من فضلك اختر صورة صحيحة";
                return RedirectToAction("Settings");
            }

            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            string ext = Path.GetExtension(file.FileName).ToLower();

            if (!validExtensions.Contains(ext))
            {
                TempData["ErrorFile"] = "الملف غير مسموح. الرجاء رفع صورة فقط (JPG - PNG - GIF - WEBP)";
                return RedirectToAction("Settings");
            }

            string imageUrl = FileUploader.UploadImage(file);
            if (imageUrl == "null")
            {
                TempData["ErrorFile"] = "فشل رفع الصوره حاول مره اخري";
                return RedirectToAction("Settings");
            }

            _parentService.UpdateImage(parentID, imageUrl);
            TempData["SucessFile"] = "تم تغيير الصوره بنجاح ";

            return RedirectToAction("Settings");
        }
        public IActionResult DeleteImage()
        {
            int userId = GetUserId();
            var parent = _parentRepo.GetByUserId(userId);
            if (parent == null) return NotFound();

            int parentId = parent.ID;

            if (!string.IsNullOrEmpty(parent.User.Image))
            {
                string imgPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    parent.User.Image.TrimStart('/')
                );

                if (System.IO.File.Exists(imgPath))
                    System.IO.File.Delete(imgPath);
            }

            parent.User.Image = "~/assets/imgs/user_image.png";

            _parentService.Update(parent);
            _parentService.Save();

            TempData["SucessFile"] = "تم حذف الصوره بنجاح ";

            return RedirectToAction("Settings");
        }

        [HttpPost]
        public IActionResult UpdateParentSetting(ParentSettingVM pVM)
        {
            int userId = GetUserId();
            var parent = _parentRepo.GetByUserId(userId);
            if (parent == null) return NotFound();

            pVM.ParentID = parent.ID;   // إجبارًا نستخدم ParentId الصحيح

            bool ok = _parentService.UpdateParentSetting(pVM);

            if (!ok)
            {
                TempData["Error"] = "حدث خطأ أثناء تحديث البيانات";
                return RedirectToAction("Settings");
            }

            TempData["Sucess"] = "تم تحديث البيانات بنجاح";
            return RedirectToAction("Settings");
        }

        public IActionResult ChangePassword(ParentSettingVM vm)
        {
            int userId = GetUserId();
            var parent = _parentRepo.GetByUserId(userId);
            if (parent == null) return NotFound();

            int parentId = parent.ID;

            bool ok = _parentService.ChangeParentPassword(parentId, vm.Password, vm.ConfirmPassword);

            if (!ok)
            {
                TempData["Error"] = "كلمه المرور غير صحيحة!!";
                return RedirectToAction("Settings");
        }

            TempData["Sucess"] = "تم تغيير كلمه المرور بنجاح";
            return RedirectToAction("Settings");
        }
    }
}
