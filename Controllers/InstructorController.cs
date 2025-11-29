using System;
using Luno_platform.Models;
using Luno_platform.Service;
using Luno_platform.Viewmodel;
//using Luno_platform.Viewmodel;
using Luno_platform.ViewModels;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

namespace Luno_platform.Controllers
{
    [Authorize(Roles = "instructor")]

    public class InstructorController : Controller
    {


        private readonly IWebHostEnvironment _env;
        public readonly Icourses_service _icourses_Service;
        private I_instructor_services _instructorService;
        private LunoDBContext _context;
        private readonly UserManager<Users> _userManager;
        public InstructorController(I_instructor_services instructorService, Icourses_service courseService, LunoDBContext context, IWebHostEnvironment env,
        UserManager<Users> userManager)
        {
            _instructorService = instructorService;
            _icourses_Service = courseService;
            _context = context;
            _env = env;
            _userManager = userManager;
        }

        public int GetInstructorIdFromUser()
        {
            // 1. جلب UserId من Login
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return -1; // مش مسجل دخول

            int userId = int.Parse(userIdClaim.Value);

            // 2. جلب الـ Instructor من الـ DbContext
            var instructor = _context.Instructors
                                     .FirstOrDefault(i => i.UserId == userId);

            if (instructor == null)
                return -1; // لا يوجد مدرس مرتبط بهذا المستخدم

            return instructor.instructorID;
        }

        public IActionResult Index()

        {
            int insrtuctorID = GetInstructorIdFromUser();
           
            var instructor = _context.Instructors
             .Include(i => i.instructor_classescs)
             .ThenInclude(ic => ic.classes)
             .ThenInclude(c => c.Courses)
             .FirstOrDefault(i => i.instructorID == insrtuctorID);

            if (instructor == null) return NotFound();

            var courses = instructor.instructor_classescs?
            .Where(ic => ic.classes != null && ic.classes.Courses != null)
            .SelectMany(ic => ic.classes.Courses)
            .Where(c => c.instructorID == insrtuctorID) // هنا بنفلتر الكورسات الخاصة بالمدرس
            .ToList() ?? new List<Courses>();



            var totalClasses = instructor.instructor_classescs.Count;
            var totalSales = courses.Sum(c => c.Student_Courses?.Count ?? 0);



            var availableCourses = courses.Count;

            var viewModel = new InstructorDashboardViewModel
            {
                Instructor = instructor ?? new Instructor(),
                Courses = courses ?? new List<Courses>(),
                //Classes = instructor.instructor_classescs?.Select(ic => ic.classes).ToList() ?? new List<Classes>(),
                Classes = instructor.instructor_classescs?
                 .Where(ic => ic.classes != null)
                  .Select(ic => new Classes
                 {
                 ClassID = ic.classes.ClassID,
                 ClassName = ic.classes.ClassName,
                 Courses = ic.classes.Courses
                   .Where(c => c.instructorID == insrtuctorID)
                   .ToList()
                }).ToList() ?? new List<Classes>(),

                TotalClasses = instructor.instructor_classescs?.Count ?? 0,
                TotalSales = totalSales,
                AvailableCourses = courses?.Count ?? 0,
                InstructorID = instructor.instructorID
            };

            return View(viewModel);
        }

        //private readonly Icourses_service _courseService;


        [HttpGet]
        public IActionResult oldAddCourse()
        {
            var model = new AddCourseViewModel
            {
                Classes = _icourses_Service.GetAllClasses(),
                SubjectsList = _icourses_Service.GetAllSubjects()
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult oldAddCourse(AddCourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Classes = _icourses_Service.GetAllClasses();
                model.SubjectsList = _icourses_Service.GetAllSubjects();
                return View(model);
            }

            // حفظ الصورة
            string imagePath = null;
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/courses");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImageFile.CopyTo(stream);
                }

                imagePath = "/images/courses/" + uniqueFileName;
            }
            int lastId = _context.Courses.Max(c => (int?)c.CourseId) ?? 32;
            int nextId = lastId + 1;


            var course = new Courses
            {
                CourseId = nextId,

                CourseName = model.Title,
                classID = model.Grade,
                SubjectId = model.SubjectId,
                price = model.Price,
                description = model.Description,
                Image = imagePath,
                createdAt = DateTime.Now,
                instructorID = GetInstructorIdFromUser()
            };

            _icourses_Service.Add(course);
            _icourses_Service.Save();

            TempData["SuccessMessage"] = "تم إضافة الكورس بنجاح!";
            return RedirectToAction("Index");
        }

        public IActionResult DeleteCourse(int courseId)
        {

            bool hasStudents = _context.Student_Courses
                .Any(sc => sc.CourseId == courseId);

            if (hasStudents)
            {
                TempData["Error"] = "لا يمكن حذف هذا الكورس لوجود طلاب مشتركين فيه.";
                return RedirectToAction("Index");
            }


            bool hasPayments = _context.Payments
                .Any(p => p.courseId == courseId);

            if (hasPayments)
            {
                TempData["Error"] = "لا يمكن حذف الكورس لأنه يحتوي على عمليات شراء.";
                return RedirectToAction("Index");
            }


            var course = _context.Courses.FirstOrDefault(c => c.CourseId == courseId);

            if (course == null)
                return NotFound();

            _context.Courses.Remove(course);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult ViewCourse(int courseId)
        {
            var course = _context.Courses

                .Include(c => c.Subjects)
                .Include(c => c.Instructor)
                .Include(c => c.CourseContent)
                .FirstOrDefault(c => c.CourseId == courseId);

            if (course == null)
                return NotFound();

            return View(course);
        }
        public IActionResult ReportsCopy()
        {
            int instructorId = GetInstructorIdFromUser();

            var vm = new ReportsPageVM
            {
                Subjects = _context.Subjects.ToList(),
                Classes = _context.Classes.ToList()
            };


            var courses = _context.Courses
                .Include(c => c.classes)
                .Include(c => c.Subjects)
                .Include(c => c.Student_Courses)
                .Where(c => c.instructorID == instructorId)
                .ToList();

            foreach (var c in courses)
            {
                vm.Courses.Add(new CourseReportVM
                {
                    CourseName = c.CourseName,
                    ClassName = c.classes.ClassName,
                    SubjectName = c.Subjects.SubjectNameAR,
                    StudentsCount = c.Student_Courses.Count,
                    Earnings = c.Student_Courses.Count * c.price
                });
            }

            return View(vm);
        }

        public JsonResult FilterCourses(int? classId, int? subjectId)
        {
            int instructorId = GetInstructorIdFromUser();

            var courses = _context.Courses
                .Include(c => c.classes)
                .Include(c => c.Subjects)
                .Include(c => c.Student_Courses)
                .Where(c => c.instructorID == instructorId);

            if (classId.HasValue)
                courses = courses.Where(c => c.classes.ClassID == classId.Value);

            if (subjectId.HasValue)
                courses = courses.Where(c => c.Subjects.SubjectID == subjectId.Value);

            var result = courses.Select(c => new
            {
                c.CourseName,
                ClassName = c.classes.ClassName,
                SubjectName = c.Subjects.SubjectNameAR,
                StudentsCount = c.Student_Courses.Count,
                Earnings = c.Student_Courses.Count * c.price
            }).ToList();

            return Json(result);
        }

       

        public IActionResult Instructorinvoices(int page = 1)
        {
            int pageSize = 10;
            int instructorId = GetInstructorIdFromUser();

            // فلترة فواتير المدرّس فقط
            var invoices = _context.Payments
                .Include(p => p.Courses)
                .Where(p => p.Courses.instructorID == instructorId)
                .OrderByDescending(p => p.date);

            // الباجيناشن
            var paginated = invoices
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new InvoiceVM
                {
                    InvoiceId = p.ID,
                    CourseName = p.Courses.CourseName,
                    Date = p.date,
                    Status = p.status,
                    Amount = p.amountPayment
                })
                .ToList();

            int totalRecords = invoices.Count();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return View(paginated);
        }

       


        [Route("Instructor/CourseDetails/{courseId}")]
        
        public IActionResult CourseDetails(int courseId)
        {
            var course = _icourses_Service.Infocourse(courseId);

            if (course == null)
            {
                return NotFound();
            }

            return View("CourseDetails", course);
        }

        [Route("Instructor/EditCourse/{courseId}")]
        public IActionResult EditCourse(int courseId)
        {
            var course = _icourses_Service.Infocourse(courseId);

            if (course == null)
                return NotFound();

            var vm = new EditCourseVM
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Description = course.description,
                Price = course.price,
                Image = course.Image,

                NameUrl1 = course.CourseContent?.nameurl1,
                Url1 = course.CourseContent?.Url1,
                NameUrl2 = course.CourseContent?.nameurl2,
                Url2 = course.CourseContent?.Url2,
                NameUrl3 = course.CourseContent?.nameurl3,
                Url3 = course.CourseContent?.Url3,

                ExamID = course.CourseContent?.Exams?.ExamID,
                ExamName = course.CourseContent?.Exams?.ExamName,

                TaskID = course.CourseContent?.Tasks?.TaskID,
                TaskName = course.CourseContent?.Tasks?.TaskName
            };

            return View(vm);
        }
        [HttpPost]
        [Route("Instructor/EditCourse/{courseId}")]
        public IActionResult EditCourse(EditCourseVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var course = _context.Courses
                .Include(c => c.CourseContent)
                .ThenInclude(cc => cc.Exams)
                .Include(c => c.CourseContent)
                .ThenInclude(cc => cc.Tasks)
                .FirstOrDefault(c => c.CourseId == model.CourseId);

            if (course == null)
                return NotFound();

            // تحديث بيانات الكورس الأساسية
            course.CourseName = model.CourseName;
            course.description = model.Description;
            course.price = model.Price;

            // التعامل مع الصورة الجديدة (رفع ملف)
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var path = Path.Combine(_env.WebRootPath, "images/courses", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                    file.CopyTo(stream);

                course.Image = "/images/courses/" + fileName;
            }
            // لو مش رفع صورة، يفضل الصورة القديمة موجودة بدون تغيير

            // تحديث محتوى الكورس
            if (course.CourseContent != null)
            {
                course.CourseContent.nameurl1 = model.NameUrl1;
                course.CourseContent.Url1 = model.Url1;

                course.CourseContent.nameurl2 = model.NameUrl2;
                course.CourseContent.Url2 = model.Url2;

                course.CourseContent.nameurl3 = model.NameUrl3;
                course.CourseContent.Url3 = model.Url3;

                if (course.CourseContent.Exams != null)
                    course.CourseContent.Exams.ExamName = model.ExamName;

                if (course.CourseContent.Tasks != null)
                    course.CourseContent.Tasks.TaskName = model.TaskName;
            }

            _context.SaveChanges();

            return RedirectToAction("CourseDetails", new { courseId = model.CourseId });
        }

        [Route("instructor/ShowCoursesTeacher/{structorid}/{classId}")]
        public IActionResult ShowCoursesTeacher(int structorid, int classId)
        {
            // جلب اسم الصف
            var className = _context.Classes
                                    .Where(c => c.ClassID == classId)
                                    .Select(c => c.ClassName)
                                    .FirstOrDefault() ?? "الصف";

            // جلب كورسات المدرس لهذا الصف
            var courses = _icourses_Service.showAllcoursebyclassandinstructor(structorid, classId);

            // إرسال الاسم مع الكورسات للـ View
            var model = new ShowCoursesTeacherVM
            {
                Courses = courses,
                ClassName = className
            };

            return View(model);
        }
        private void PopulateDropdowns()
        {
            var subjects= _context.Subjects.ToList() ?? new List<Subject>();
            var classes = _context.Classes.ToList() ?? new List<Classes>();

            ViewBag.Subjects = new SelectList(subjects, "SubjectID", "SubjectName");
            ViewBag.Classes = new SelectList(classes, "ClassID", "ClassName");
        }

        [HttpGet]
        [Route("Instructor/AddCourse")]
        public IActionResult AddCourse()
        {
            var vm = new AddCourseVM
            {
                Subjects = _context.Subjects.Select(s => new SelectListItem
                {
                    Value = s.SubjectID.ToString(),
                    Text = s.SubjectNameEN
                }).ToList(),

                Classes = _context.Classes.Select(c => new SelectListItem
                {
                    Value = c.ClassID.ToString(),
                    Text = c.ClassName
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [Route("Instructor/AddCourse")]
        public IActionResult AddCourse(AddCourseVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Subjects = _context.Subjects.Select(s => new SelectListItem
                {
                    Value = s.SubjectID.ToString(),
                    Text = s.SubjectNameEN
                }).ToList();

                model.Classes = _context.Classes.Select(c => new SelectListItem
                {
                    Value = c.ClassID.ToString(),
                    Text = c.ClassName
                }).ToList();

                return View(model);
            }

            int instructorId = GetInstructorIdFromUser();

            // جلب الصف من الداتابيز
            var cls = _context.Classes.FirstOrDefault(c => c.ClassID == model.ClassID);
            if (cls == null)
            {
                TempData["Error"] = "الصف غير موجود. الرجاء اختيار صف صالح.";
                return RedirectToAction("AddCourse");
            }

            // توليد CourseId
            int lastId = _context.Courses.Max(c => (int?)c.CourseId) ?? 0;
            int nextId = lastId + 1;

            // إنشاء الكورس
            var course = new Courses
            {
                CourseId = nextId,
                CourseName = model.CourseName,
                description = model.Description,
                price = model.Price,
                SubjectId = model.SubjectId,
                classID = cls.ClassID,
                instructorID = instructorId
            };

            // حفظ الصورة
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/courses", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                model.ImageFile.CopyTo(stream);
                course.Image = "/images/courses/" + fileName;
            }

            _context.Courses.Add(course);
            _context.SaveChanges();

            // إنشاء العلاقة بين المدرس والصف لو مش موجودة
            bool relationExists = _context.instructor_classescs
                .Any(ic => ic.instructorId == instructorId && ic.classId == cls.ClassID);

            if (!relationExists)
            {
                var instructorClass = new instructor_classescs
                {
                    instructorId = instructorId,
                    classId = cls.ClassID
                };
                _context.instructor_classescs.Add(instructorClass);
                _context.SaveChanges();
            }
       

            // حفظ محتوى الكورس
            var content = new CourseContent
            {
                cousrsid = course.CourseId,
                nameurl1 = model.NameUrl1,
                Url1 = model.Url1,
                nameurl2 = model.NameUrl2,
                Url2 = model.Url2,
                nameurl3 = model.NameUrl3,
                Url3 = model.Url3
            };

            _context.CourseContents.Add(content);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        //public IActionResult CourseDetails(int id)
        //{
        //    var course = _context.Courses
        //        .Include(c => c.Instructor)
        //        .Include(c => c.Weeks)
        //            .ThenInclude(w => w.Lessons)
        //        .FirstOrDefault(c => c.CourseId == id);

        //    if (course == null)
        //        return NotFound();

        //    var viewModel = new CourseDetailsViewModel
        //    {
        //        CourseId = course.CourseId,
        //        CourseName = course.CourseName,
        //        Description = course.description,
        //        Price = course.Price,
        //        StudentsCount = course.StudentsCount,
        //        TeacherName = course.Instructor.FullName,
        //        CourseImage = course.ImageUrl,
        //        Weeks = course.Weeks.Select(w => new WeekViewModel
        //        {
        //            WeekId = w.WeekId,
        //            WeekName = w.WeekName,
        //            Lessons = w.Lessons.Select(l => new LessonViewModel
        //            {
        //                LessonId = l.LessonId,
        //                LessonTitle = l.Title,
        //                LessonDescription = l.Description
        //            }).ToList()
        //        }).ToList()
        //    };

        public IActionResult Settings()
        {
            int instructorId = GetInstructorIdFromUser();

            if (instructorId == -1)
                return RedirectToAction("Login", "Account"); // أو Unauthorized()

            var instructor = _context.Instructors
                .Include(i => i.User)
                .Include(i => i.Subject)
                .FirstOrDefault(i => i.instructorID == instructorId);

            if (instructor == null)
                return NotFound();

            var vm = new InstructorSettingsVM
            {
                InstructorID = instructor.instructorID,
                UserId = instructor.User.Id,   // ✅ مهم للباسورد

                FirstName = instructor.User.fname,
                LastName = instructor.User.lastName,
                Email = instructor.User.Email,
                PhoneNumber = instructor.User.PhoneNumber,

                Image = string.IsNullOrEmpty(instructor.User.Image)
                    ? "/assets/img/default-avatar.png"
                    : instructor.User.Image,

                Motto = instructor.motto,
                Bio = instructor.bio,
                Eligible = instructor.eligible,

                SubjectID = instructor.SubjectID,
                SubjectName = instructor.Subject?.SubjectNameAR,

                Notifications = new List<string>
        {
            "تم إضافة طالب جديد",
            "تم شراء كورس جديد"
        },

                NotificationSettings = new List<NotificationSettingVM>
        {
            new NotificationSettingVM { Label = "رسائل جديدة", IsEnabled = true },
            new NotificationSettingVM { Label = "تنبيهات المنصة", IsEnabled = false }
        }
            };

            return View(vm);
        }


        [HttpPost]
        public IActionResult UpdateSettings(InstructorSettingsVM model)
        {
            var instructor = _context.Instructors
                .Include(i => i.User)
                .FirstOrDefault(i => i.instructorID == model.InstructorID);

            if (instructor == null) return NotFound();

            // Users
            instructor.User.fname = model.FirstName;
            instructor.User.lastName = model.LastName;
            instructor.User.Email = model.Email;
            instructor.User.PhoneNumber = model.PhoneNumber;

            // Instructor
            instructor.motto = model.Motto;
            instructor.bio = model.Bio;
            instructor.eligible = model.Eligible;

            // صورة جديدة؟
            if (model.ImageFile != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "images");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);

                string path = Path.Combine(folder, fileName);

                using (var fs = new FileStream(path, FileMode.Create))
                {
                    model.ImageFile.CopyTo(fs);
                }

                instructor.User.Image = "/images/" + fileName;
            }

            _context.SaveChanges();

            return RedirectToAction("Settings");
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(InstructorSettingsVM model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId.ToString());

            if (user == null)
                return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return RedirectToAction("Settings");
            }

            return RedirectToAction("Settings");
        }
        [HttpPost]
        public IActionResult UpdateProfileImage(int InstructorID, IFormFile ImageFile)
        {
            var instructor = _context.Instructors
                .Include(i => i.User)
                .FirstOrDefault(i => i.instructorID == InstructorID);

            if (instructor == null) return NotFound();

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "images");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);

                string path = Path.Combine(folder, fileName);

                using (var fs = new FileStream(path, FileMode.Create))
                {
                    ImageFile.CopyTo(fs);
                }

                instructor.User.Image = "/images/" + fileName;

                _context.SaveChanges();
            }

            return RedirectToAction("Settings");
        }

    }

}







