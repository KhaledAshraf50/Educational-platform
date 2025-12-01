using Luno_platform.Helpers;
using Luno_platform.Service;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Luno_platform.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
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
        
        [Route("/Admin/mainpage")]
        public IActionResult mainpage()
        {
            // جلب الـ UserId من الـ Claims (أو من الـ session حسب تطبيقك)
            var userId = GetUserId();
            //var userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);

            var model = _adminService.GetDashboardData(userId);

            return View(model);
        }
        public IActionResult users()
        {
            return View();
        }
        public IActionResult courses()
        {
            return View();
        }
        public IActionResult payments()
        {
            return View();
        }
        public IActionResult Report()
        {
            return View();
        }
        public IActionResult Main()
        {
            return View();
        }
        public IActionResult setting()
        {
            int userId = GetUserId();

            var vm = _adminService.GetAdminSetting(userId);

            if (vm == null)
                return NotFound();

            return View(vm);
        }
        [HttpPost]
        public IActionResult UploadImage(IFormFile file)
        {
            int userId = GetUserId();
            //var admin = _adminService.GetByUserId(userId);// لحد ما نعمل تسجيل لادمين
            //if (parent == null) return NotFound();

            //int parentID = parent.ID;

            if (file == null || file.Length == 0)
            {
                TempData["ErrorFile"] = "من فضلك اختر صورة صحيحة";
                return RedirectToAction("setting");
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

            _adminService.UpdateImage(userId, imageUrl);
            TempData["SucessFile"] = "تم تغيير الصوره بنجاح ";
            return RedirectToAction("setting");
        }
        public IActionResult DeleteImage()
        {
            int userId = GetUserId();
            var admin = _adminService.GetAdmin(userId);
            if (admin == null) return NotFound();
            if (!string.IsNullOrEmpty(admin.User.Image))
            {
                string imgPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    admin.User.Image.TrimStart('/')
                );

                if (System.IO.File.Exists(imgPath))
                    System.IO.File.Delete(imgPath);
            }

            admin.User.Image = "~/assets/imgs/user_image.png";

            _adminService.Update(admin);
            _adminService.Save();

            TempData["SucessFile"] = "تم حذف الصوره بنجاح ";

            return RedirectToAction("setting");
        }

        [HttpPost]
        public IActionResult UpdateAdminSetting(AdminSettingVM AVM)
        {
            int userId = GetUserId();
            var admin = _adminService.GetAdmin(userId);
            if (admin == null) return NotFound();

            //AVM.AdminId = admin.ID;   
            
            bool ok = _adminService.UpdateAdminSetting(AVM);

            if (!ok)
            {
                TempData["Error"] = "حدث خطأ أثناء تحديث البيانات";
                return RedirectToAction("Settings");
            }

            TempData["Sucess"] = "تم تحديث البيانات بنجاح";
            return RedirectToAction("setting");
        }

        public IActionResult ChangePassword(AdminSettingVM AVM)
        {
            int userId = GetUserId();
            var admin = _adminService.GetAdmin(userId);
            if (admin == null) return NotFound();

            //int adminId = admin.ID;
            bool ok = _adminService.ChangeAdminPassword(AVM.AdminId, AVM.CurrentPassword, AVM.ConfirmNewPassword);
            if (!ok)
            {
                TempData["Error"] = "كلمه المرور غير صحيحة!!";
                return RedirectToAction("Settings");
            }

            TempData["Sucess"] = "تم تغيير كلمه المرور بنجاح";
            return RedirectToAction("setting");
        }
    }
}
