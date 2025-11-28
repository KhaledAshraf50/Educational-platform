using System;
using Luno_platform.Models;
using Luno_platform.Service;
using Luno_platform.Viewmodel;
using Luno_platform.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Controllers
{
    public class InstructorController : Controller
    {




        private I_instructor_services _instructorService;
        private LunoDBContext _context;

        public InstructorController(I_instructor_services instructorService, Icourses_service courseService, LunoDBContext context)
        {
            _instructorService = instructorService;
            _courseService = courseService;
            _context = context;
        }


        public IActionResult Index(int instructorId)

        {
            instructorId = 1;

            var instructor = _context.Instructors
             .Include(i => i.instructor_classescs)
             .ThenInclude(ic => ic.classes)
             .ThenInclude(c => c.Courses)
             .FirstOrDefault(i => i.instructorID == instructorId);

            if (instructor == null) return NotFound();

            var courses = instructor.instructor_classescs?
                .Where(ic => ic.classes != null && ic.classes.Courses != null)
                .SelectMany(ic => ic.classes.Courses)
                .ToList() ?? new List<Courses>();



            var totalClasses = instructor.instructor_classescs.Count;
            var totalSales = courses.Sum(c => c.Student_Courses?.Count ?? 0);



            var availableCourses = courses.Count;

            var viewModel = new InstructorDashboardViewModel
            {
                Instructor = instructor ?? new Instructor(),
                Courses = courses ?? new List<Courses>(),
                Classes = instructor.instructor_classescs?.Select(ic => ic.classes).ToList() ?? new List<Classes>(),
                TotalClasses = instructor.instructor_classescs?.Count ?? 0,
                TotalSales = totalSales,
                AvailableCourses = courses?.Count ?? 0
            };

            return View(viewModel);
        }

        private readonly Icourses_service _courseService;


        [HttpGet]
        public IActionResult AddCourse()
        {
            var model = new AddCourseViewModel
            {
                Classes = _courseService.GetAllClasses(),
                SubjectsList = _courseService.GetAllSubjects()
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCourse(AddCourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Classes = _courseService.GetAllClasses();
                model.SubjectsList = _courseService.GetAllSubjects();
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
                instructorID = 1
            };

            _courseService.Add(course);
            _courseService.Save();

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
            int instructorId = 1;

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
            int instructorId = 1;

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

        public async Task<IActionResult> coursesDetails(int id)
        {
            // 1. جلب بيانات الكورس الأساسية من جدول Courses
            var course = await _context.Courses
                .Include(c => c.Instructor) // لجلب بيانات المدرس
                .ThenInclude(i => i.User)   // لجلب اسم المستخدم (المدرس)
                .Include(c => c.Student_Courses) // لحساب عدد الطلاب
                .Include(c => c.CourseContent) // لجلب محتوى الكورس (الدروس/الأسابيع)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            // 2. تحويل البيانات إلى CourseDetailsViewModel
            var viewModel = new CourseDetailsViewModel
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                CourseImage = course.Image ?? "/path/to/default/image.jpg", // استخدام صورة افتراضية إذا لم تتوفر
                TeacherName = course.Instructor?.User?.fname + " " + course.Instructor?.User?.lastName,
                Price = course.price,
                StudentsCount = course.Student_Courses?.Count ?? 0,
                Description = course.description,
                TeacherId = course.instructorID
            };

            // 3. تحويل محتوى الكورس (CourseContent) إلى Weeks و Lessons
            // بما أن CourseContent هو كيان واحد مرتبط بـ Courses، سنقوم بمعاملته كـ "أسبوع واحد"
            if (course.CourseContent != null)
            {
                var week = new WeekViewModel
                {
                    // نستخدم CourseContent.Id كـ WeekId
                    WeekId = course.CourseContent.Id,
                    // نستخدم CourseContent.nameurl1 كـ WeekName
                    WeekName = !string.IsNullOrEmpty(course.CourseContent.nameurl1) ? course.CourseContent.nameurl1 : "محتوى الدورة"
                };

                int lessonCounter = 1;

                // إضافة الدرس الأول (Url1)
                if (!string.IsNullOrEmpty(course.CourseContent.Url1))
                {
                    week.Lessons.Add(new LessonViewModel
                    {
                        LessonId = lessonCounter++,
                        LessonTitle = course.CourseContent.nameurl1 ?? "الدرس الأول"
                    });
                }

                // إضافة الدرس الثاني (Url2)
                if (!string.IsNullOrEmpty(course.CourseContent.Url2))
                {
                    week.Lessons.Add(new LessonViewModel
                    {
                        LessonId = lessonCounter++,
                        LessonTitle = course.CourseContent.nameurl2 ?? "الدرس الثاني"
                    });
                }

                // إضافة الدرس الثالث (Url3)
                if (!string.IsNullOrEmpty(course.CourseContent.Url3))
                {
                    week.Lessons.Add(new LessonViewModel
                    {
                        LessonId = lessonCounter++,
                        LessonTitle = course.CourseContent.nameurl3 ?? "الدرس الثالث"
                    });
                }

                // يمكنك إضافة مهام (Tasks) وامتحانات (Exams) كـ "دروس" إضافية هنا إذا كانت مطلوبة في الـ View
                // بناءً على حقول ExamId و taskId في CourseContent.

                viewModel.Weeks.Add(week);
            }
            // إذا كان المطلوب هو عدة CourseContent لكل كورس:
            /* // إذا كان الكيان Courses يحتوي على ICollection<CourseContent> وليس CourseContent واحد فقط:
            if (course.CourseContents != null)
            {
                foreach (var content in course.CourseContents)
                {
                    // تحويل كل محتوى CourseContent إلى WeekViewModel
                    ...
                }
            }
            */

            return View(viewModel);
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

        //    return View(viewModel);
        //}



        //// حفظ التعديلات
        //[HttpPost]
        //public IActionResult SaveChanges(CourseDetailsViewModel model)
        //{
        //    if (!ModelState.IsValid) return View("Details", model);

        //    var course = _context.Courses
        //        .Include(c => c.CourseContent)
        //        .FirstOrDefault(c => c.CourseId == model.CourseId);

        //    if (course == null) return NotFound();

        //    course.CourseName = model.CourseName;
        //    course.description = model.Description;

        //    if (course.CourseContent != null && model.Contents.Any())
        //    {
        //        var content = course.CourseContent;
        //        var contentVm = model.Contents.First();

        //        content.nameurl1 = contentVm.NameUrl1;
        //        content.Url1 = contentVm.Url1;
        //        content.nameurl2 = contentVm.NameUrl2;
        //        content.Url2 = contentVm.Url2;
        //        content.nameurl3 = contentVm.NameUrl3;
        //        content.Url3 = contentVm.Url3;
        //    }

        //    _context.SaveChanges();
        //    TempData["Success"] = "تم حفظ التعديلات بنجاح!";
        //    return RedirectToAction("Details", new { id = model.CourseId });
        //}

        public IActionResult Instructorinvoices(int page = 1)
        {
            int pageSize = 10;
            int instructorId = 1;

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

    }

}







