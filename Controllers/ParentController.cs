using Luno_platform.Helpers;
using Luno_platform.Models;
using Luno_platform.Repository;
using Luno_platform.Service;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Mvc;

namespace Luno_platform.Controllers
{
    public class ParentController : Controller
    {
        IParentService _parentService;
        public ParentController(IParentService parentService)
        {
            _parentService = parentService;
        }
        public IActionResult MainPage(int id)
        {
            var parent = _parentService.GetParent(id);
            return View(parent);
        }
        //------------------------
        public IActionResult Childerns(int id)
        {
            var vm = new ParentChildrenViewModel
            {
                ParentID = id,
                Students = _parentService.GetStds(id)
            };
            return View(vm);
        }
        public IActionResult ChildDetails(int id)
        {
            if (id <= 0)
                return RedirectToAction("Childerns");
            Student std = _parentService.GetStudentDetails(id);
            if (std == null)
                return RedirectToAction("Childerns");
            return View(std);
        }
        public IActionResult AddChild(int parentId)
        {
            ChildViewModel vm = new ChildViewModel();
            vm.ParentID = parentId;
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
            return View();
        }
        //------------------------
        public IActionResult Settings(int ParentId)
        {
            var vm = _parentService.GetParentSetting(ParentId);
            return View(vm);
        }
   
        [HttpPost]
        public IActionResult UploadImage(int parentID, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorFile"] = "من فضلك اختر صورة صحيحة";
                return RedirectToAction("Settings", new { ParentId = parentID });
            }
            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            string ext = Path.GetExtension(file.FileName).ToLower();
            if(!validExtensions.Contains(ext))
            {
                TempData["ErrorFile"] = "الملف غير مسموح. الرجاء رفع صورة فقط (JPG - PNG - GIF - WEBP)";
                return RedirectToAction("Settings", new { ParentId = parentID });
            }
            string imageUrl = FileUploader.UploadImage(file);
            if (imageUrl == "null")
            {
                TempData["ErrorFile"] = "فشل رفع الصوره حاول مره اخري";
                return RedirectToAction("Settings", new { ParentId = parentID });
            }
            _parentService.UpdateImage(parentID, imageUrl);
            TempData["SucessFile"] = "تم تغيير الصوره بنجاح ";
            return RedirectToAction("Settings", new { ParentId = parentID });
        }
        public IActionResult DeleteImage(int parentId)
        {
            var parent = _parentService.GetById(parentId);
            if (parent == null)
            {
                TempData["ErrorFile"] = "المستخدم غير موجود";
                return RedirectToAction("Settings", new { ParentId = parentId });
            }
            //if (!string.IsNullOrEmpty(parent.Image))
            //{
            //    string imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", parent.Image.TrimStart('/'));
            //    if (System.IO.File.Exists(imgPath))
            //    {
            //        System.IO.File.Delete(imgPath);
            //    }
            //}
            //parent.Image = "/assets/imgs/default.png";
            _parentService.Update(parent);
            _parentService.Save();
            TempData["SucessFile"] = "تم حذف الصوره بنجاح ";
            return RedirectToAction("Settings", new { ParentId = parentId });
        }
        [HttpPost]
        public IActionResult UpdateParentSetting(ParentSettingVM pVM)
        {
            bool ok =_parentService.UpdateParentSetting(pVM);
            if (!ok)
            {
                TempData["Error"] = "حدث خطأ أثناء تحديث البيانات";
                return RedirectToAction("Settings", new { ParentId = pVM.ParentID });
            }
            TempData["Sucess"] = "تم تحديث البيانات بنجاح";
            return RedirectToAction("Settings", new { parentId = pVM.ParentID });
        }
        public IActionResult ChangePassword(ParentSettingVM vm)
        {
            bool ok = _parentService.ChangeParentPassword(vm.ParentID, vm.Password, vm.ConfirmPassword);
            if (!ok)
            {
                TempData["Error"] = "كلمه المرور غير صحيحة!!";
                return RedirectToAction("Settings", new { parentId = vm.ParentID });
            }
            TempData["Sucess"] = "تم تغيير كلمه المرور بنجاح ";
            return RedirectToAction("Settings", new { parentId = vm.ParentID });
        }

    }
}
