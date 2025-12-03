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
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account"); // بعد الخروج هيرجع لصفحة تسجيل الدخول
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
            if (user.status == "Pending")
            {
                return View("AccountPending");
            }

            // 3) نجاح الدخول
            return RedirectToAction("mainpage", "Homepage");
        }




        [HttpGet] 
        public IActionResult role()
        {
            return View(); 
        }








        private async Task<(bool Success, Users User, IEnumerable<IdentityError> Errors)>
CreateBaseUser(Register_User_Viewmode model)
        {

            // التحقق من الرقم القومي موجود قبل
            if (_context.Users.Any(u => u.nationalID == model.NationalID))
            {
                var error = new IdentityError { Description = "الرقم القومي موجود من قبل" };
                return (false, null, new List<IdentityError> { error });
            }
            var user = new Users
            {
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                fname = model.Fname,
                lastName = model.LastName,
                role = model.Role.ToLower(),
                nationalID = model.NationalID,
                PasswordHash = model.Password,
                Image = model.Image ?? "~/assets/imgs/user_image.png",
                UserName = model.Email,
                CreatedAt = DateOnly.FromDateTime(DateTime.Today),
                
                
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return (false, user, result.Errors);

            await _userManager.AddToRoleAsync(user, model.Role);

            return (true, user, null);
        }





        [HttpGet]
        public IActionResult RegisterStudent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterStudent(Register_User_Viewmode model)
        {
            model.Role = "student";

            if (!ModelState.IsValid)
                return View(model);

            var (success, user, errors) = await CreateBaseUser(model);

            if (!success)
            {
                foreach (var err in errors)
                    ModelState.AddModelError("", err.Description);

                return View(model);
            }

            var student = new Student
            {
                UserId = user.Id,
                classId = model.ClassId ?? 1,
                branch = model.branch,
                parentnumber = model.parentnumber,
                goverment = model.goverment,
                city = model.city
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Account");
        }





        //----------------------------
        [HttpGet]
        public IActionResult RegisterInstructor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterInstructor(Register_User_Viewmode model)
        {
            model.Role = "instructor";

            if (!ModelState.IsValid)
                return View(model);

            var (success, user, errors) = await CreateBaseUser(model);

            if (!success)
            {
                foreach (var err in errors)
                    ModelState.AddModelError("", err.Description);

                return View(model);
            }

            var instructor = new Instructor
            {
                UserId = user.Id,
                SubjectID = model.SubjectID ?? 0,
                motto = model.Motto,
                bio = model.Bio,
                eligible = model.Eligible
            };

            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Account");
        }

        //_________________________

        [HttpGet]
        public IActionResult RegisterParent()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterParent(Register_User_Viewmode model)
        {
            model.Role = "parent";

            if (!ModelState.IsValid)
                return View(model);

            var (success, user, errors) = await CreateBaseUser(model);

            if (!success)
            {
                foreach (var err in errors)
                    ModelState.AddModelError("", err.Description);

                return View(model);
            }

            var parent = new Parent
            {
                UserId = user.Id
            };

            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Account");
        }

        //_____________

        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAdmin(Register_User_Viewmode model)
        {
            model.Role = "admin";

            // تحقق من باسورد الأدمن
            if (model.Passwordregiester != "2025")
            {
                ModelState.AddModelError("Passwordregiester", "كلمة السر الخاصة بالأدمن غير صحيحة");
                return View(model);
            }

            if (!ModelState.IsValid)
                return View(model);

            var (success, user, errors) = await CreateBaseUser(model);

            if (!success)
            {
                foreach (var err in errors)
                    ModelState.AddModelError("", err.Description);

                return View(model);
            }

            var admin = new Admin
            {
                UserId = user.Id
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login", "Account");
        }


    }
}

        