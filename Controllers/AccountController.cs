using Luno_platform.Models;
using Luno_platform.Viewmodel;
using Luno_platform.Viewmodel.Account_viewmode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Luno_platform.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly LunoDBContext _context;
        private readonly IWebHostEnvironment _env;

        public AccountController(
            UserManager<Users> userManager,
            SignInManager<Users> signInManager,
            LunoDBContext context,
            IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _env = env;
        }


        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account"); // بعد الخروج هيرجع لصفحة تسجيل الدخول
        }



        [HttpGet]
        public IActionResult register2()
        {
            return View("register2");
        }













        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }
       
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // 1) نجيب اليوزر
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Email not found");
                return View(model);
            }

            // 2) تسجيل الدخول فعليًا + إنشاء Cookie
            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,           // مهم جدًا: هنا اليوزر نيم مش اليوزر نفسه
                model.Password,
                model.RememberMe,        // تفعيل Remember Me
                lockoutOnFailure: false
            );

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid password");
                return View(model);
            }

            // 3) نجاح الدخول
            return RedirectToAction("mainpage", "Homepage");
        }

        // ===========================
        // عرض صفحة التسجيل
        // ===========================
        [HttpGet]

        public IActionResult Register()
        {
            var model = new Register_User_Viewmode(); // نموذج فارغ
            return View(model);
        }

        // ===========================
        // معالجة التسجيل
        // ===========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register_User_Viewmode model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // إنشاء اليوزر الأساسي
            var user = new Users
            {
              
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                fname = model.Fname,
                lastName = model.LastName,
                role = model.Role.ToLower(),
                nationalID = model.NationalID,
                PasswordHash=model.Password,
                Image=model.Image ??"~/assets/imgs/user_image.png",
                UserName=model.Email


               
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            // إضافة Role
            await _userManager.AddToRoleAsync(user, model.Role);

            // حسب الـ Role نضيف البيانات الخاصة
            switch (model.Role.ToLower())
            {
                case "student":
                    var student = new Student
                    {
                        
                        UserId = user.Id,
                        classId = model.ClassId ?? 1,
                        branch = model.branch,
                        parentnumber=model.parentnumber,
                        goverment=model.goverment,
                        city=model.city

                    };
                    _context.Students.Add(student);
                    break;

                case "instructor":
                    var instructor = new Instructor
                    {
                        UserId = user.Id,
                        SubjectID = model.SubjectID ?? 0,
                        motto = model.Motto,
                        bio = model.Bio,
                        eligible = model.Eligible
                    };
                    _context.Instructors.Add(instructor);
                    break;

                case "parent":
                    var parent = new Parent
                    {
                        UserId = user.Id
                    };
                    _context.Parents.Add(parent);
                    break;
            }

            await _context.SaveChangesAsync(); // الحفظ النهائي لكل شيء
            return RedirectToAction("Login", "Account");
        }


        // ===========================
        // إنشاء طالب
        // ===========================
        private async Task CreateStudent(int userId, Register_User_Viewmode model)
        {
            var student = new Student
            {
                UserId = userId,
                classId = model.ClassId ?? 1,
                //ParentId = model.ParentId,
                //branch = model.Shueba
            };

            _context.Students.Add(student);
        }

        // ===========================
        // إنشاء مدرس
        // ===========================
        private async Task CreateTeacher(int userId, Register_User_Viewmode model)
        {
            //string imagePath = null;

            //if (model.TeacherImage != null)
            //{
            //    imagePath = await SaveImage(model.TeacherImage, "teachers");
            //}

            //var instructor = new Instructor
            //{
            //    UserId = userId,
            //    SubjectID = model.SubjectID ?? 0,
            //    motto = model.Motto,
            //    bio = model.Bio,
            //    eligible = model.Eligible,
            //    Image = imagePath
            //};

            //_context.Instructors.Add(instructor);
        }

        // ===========================
        // إنشاء ولي أمر
        // ===========================
        private async Task CreateParent(int userId, Register_User_Viewmode model)
        {
            //string imagePath = null;

            //if (model.ParentImage != null)
            //{
            //    imagePath = await SaveImage(model.ParentImage, "parents");
            //}

            //var parent = new Parent
            //{
            //    UserId = userId,
            //    Image = imagePath
            //};

            //_context.Parents.Add(parent);
        }

        // ===========================
        // حفظ الصور
        // ===========================
        //private async Task<string> SaveImage(IFormFile file, string folder)
        //{
        //    if (file == null) return null;

        //    string uploadsFolder = Path.Combine(_env.WebRootPath, "images", folder);
        //    Directory.CreateDirectory(uploadsFolder);

        //    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        //    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //    using (var fileStream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(fileStream);
        //    }

        //    return $"/images/{folder}/{uniqueFileName}";
        //}



    }
}