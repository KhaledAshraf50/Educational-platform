using Luno_platform.Service;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Mvc;

namespace Luno_platform.Controllers
{
    public class CoursesController : Controller
    {
        public readonly Icourses_service _courses;

        public CoursesController(Icourses_service courses)
        {
            _courses = courses;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Filter()
        {
            var model = new filter_coursesviewmodel
            {
                Courses = _courses.showallcourses()// جلب كل الكورسات في البداية
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Filter(filter_coursesviewmodel filter)
        {
            // جلب كل الكورسات
            var courses = _courses.showallcourses().AsQueryable();

            // فلترة باسم الكورس
            if (!string.IsNullOrEmpty(filter.coursesname))
            {
                courses = courses.Where(c => c.CourseName.Contains(filter.coursesname));
            }

            // فلترة حسب المادة
            if (filter.subjectid.HasValue)
            {
                courses = courses.Where(c => c.SubjectId == filter.subjectid.Value);
            }

            // فلترة حسب المستوى
            if (filter.Level.HasValue && filter.Level.Value != 0)
            {
                courses = courses.Where(c => c.classID == filter.Level.Value);
            }

            // فلترة حسب السعر
            if (filter.MinPrice.HasValue)
            {
                courses = courses.Where(c => c.price >= filter.MinPrice.Value);
            }
            if (filter.MaxPrice.HasValue)
            {
                courses = courses.Where(c => c.price <= filter.MaxPrice.Value);
            }

            // إرجاع النتائج مع الاحتفاظ بقيم الفلاتر
            filter.Courses = courses.ToList();

            return View(filter);
        }

        // Action للصفحة الأولى
       
     
    }
    }








