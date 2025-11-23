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
            Student std = _parentService.GetStudentDetails(id);
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
                ImageUrl = student.Image,
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
        public IActionResult Settings(int ParentId)
        {
            var vm=_parentService.GetParentSetting(ParentId);
            return View(vm);
        }
        public IActionResult ChangePassword(ParentSettingVM vm)
        {
            bool ok = _parentService.ChangeParentPassword(vm.ParentID, vm.Password, vm.ConfirmPassword);
            if (!ok)
            {
                TempData["Error"] = "كلمه المرور غير صحيحة!!";
                return RedirectToAction("Settings");
            }
            TempData["Sucess"] = "تم تغيير كلمه المرور بنجاح ";
            return RedirectToAction("Settings");
        }
        [HttpPost]
        public IActionResult UploadImage(int parentID,IFormFile file)
        {
            string imageUrl = FileUploader.UploadImage(file);
            _parentService.UpdateImage(parentID, imageUrl);
            if (imageUrl == "null")
            {
                TempData["ErrorFile"] = "فشل رفع الصوره حاول مره اخري";
                return RedirectToAction("Settings", new { ParentId = parentID });
            }
            TempData["SucessFile"] = "تم تغيير الصوره بنجاح ";
            return RedirectToAction("Settings", new { ParentId = parentID });
        }

    }
}
