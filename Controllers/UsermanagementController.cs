using Luno_platform.Service;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Mvc;

namespace Luno_platform.Controllers
{
    public class UsermanagementController : Controller
    {
        private readonly IstudentService _istudent;

        private readonly I_instructor_services _Instructor;
        private readonly IParentService _parent;
        private readonly IUserservecs _userservecs;


        public UsermanagementController( IstudentService istudent , I_instructor_services instructor, IParentService parent ,IUserservecs userservecs)
        {
            _istudent = istudent;
            _Instructor = instructor;
            _parent = parent;
            _userservecs = userservecs;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult students(int page = 1)
        {
            int pageSize = 7;

            var allStudents = _istudent.showStudents(); 

            var pagedStudents = allStudents
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            int totalPages = (int)Math.Ceiling(allStudents.Count / (double)pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedStudents);
            
        }


        public IActionResult deletestudent(int userid)
        {
            _istudent.DeleteStudent(userid);

            return RedirectToAction("students");
        }

        
        public IActionResult SetstudentPending(int userid)
        {
            _istudent.SetUserPending(userid);

            return RedirectToAction("students");
        }



        //______________________________________________________________-



        public IActionResult instructors(int page =1)
        {
            int pageSize = 7;

            var allInstructor = _Instructor.showinstructor();

            var pagedInstructor = allInstructor
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            int totalPages = (int)Math.Ceiling(allInstructor.Count / (double)pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedInstructor);
        }




        public IActionResult deleteinstructor(int userid)
        {
            _Instructor.deleteinstructor(userid);

            return RedirectToAction("instructors");
        }



        public IActionResult SetinstructorPending(int userid)
        {
            _istudent.SetUserPending(userid);

            return RedirectToAction("instructors");
        }


        //______________________________________________________________-

        public IActionResult parents(int page =1)
        {

            int pageSize = 7;

            var allParents = _parent.showparent();

            var pagedInstructor = allParents
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            int totalPages = (int)Math.Ceiling(allParents.Count / (double)pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedInstructor);
        }




        public IActionResult deleteparent(int userid)
        {
           _parent.deleteparent(userid);

            return RedirectToAction("parent");
        }


        public IActionResult SetparnetPending(int userid)
        {
            _istudent.SetUserPending(userid);

            return RedirectToAction("parents");
        }

        //______________________________________________________________-


     public IActionResult pandingusers(int page = 1)
        {
            int pageSize = 7;

            // نجيب كل ال Pending
            var pending = _userservecs.pandingusers();

            // Apply Pagination
            var pagedPending = pending
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            // حساب عدد الصفحات
            int totalPages = (int)Math.Ceiling(pending.Count / (double)pageSize);

            // ارسال بيانات الباجينج للصفحة
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedPending);
        }


        public IActionResult NotActiveUsers(int page = 1)
        {
            int pageSize = 7;

            // نجيب كل ال NotActive
            var notActive = _userservecs.NotActiveusers();

            // Apply Pagination
            var pagedNotActive = notActive
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            // حساب عدد الصفحات
            int totalPages = (int)Math.Ceiling(notActive.Count / (double)pageSize);

            // ارسال بيانات الباجينج
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedNotActive);
        }

        //
        public IActionResult SetUsersActive(int userid)
        {
            _userservecs.SetUsersActive(userid);
            TempData["AlertMessage"] = "تم تفعيل الحساب ";


            return RedirectToAction("pandingusers");
        }

        public IActionResult deletuser(int userid)
        {

            
            TempData["AlertMessage"] = _userservecs.deleteuser(userid);


            return RedirectToAction("pandingusers");
        }


        public IActionResult showAccount(int userid)
        {

            return View();
        }


        //_______________________________________
        public IActionResult UserDetails(int userId)
        {
            // 1. جلب بيانات المستخدم حسب الـ Id
            var user = _userservecs.GetUserDetails(userId);

            if (user == null)
            {
                TempData["AlertMessage"] = "المستخدم غير موجود.";
                return RedirectToAction("PendingUsers"); // أو أي صفحة ترجع لها
            }

            // 2. تحديد نوع المستخدم حسب الدور
            // نفترض إن role موجود في قاعدة البيانات ويكون "Student", "Teacher", "Parent"
            string userType = user.role;
            ViewBag.UserType = userType;

            // 3. إعادة الـ View مع بيانات المستخدم
            return View(user);
        }
    }
}
