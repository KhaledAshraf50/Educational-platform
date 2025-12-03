using Luno_platform.Models;
using Luno_platform.Viewmodel;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Luno_platform.Repository
{
    public class AdminRepository : BaseRepository<Admin>, IAdminRepository
    {
        public AdminRepository(LunoDBContext lunoDBContext) : base(lunoDBContext)
        {
        }

        //private readonly LunoDBContext _context;

        //public AdminRepository(LunoDBContext context)
        //{
        //    _context = context;
        //}

        public Users GetAdminByUserId(int userId)
        {
            return _Context.Users.FirstOrDefault(u => u.Id == userId);
        }
        public Admin GetAdmin(int userId)
        {
            return _Context.Admins.Include(u=>u.User).FirstOrDefault(u => u.UserId == userId);
        }

        public int GetTotalStudents()
        {
            return _Context.Students.Count();
        }

        public int GetTotalInstructors()
        {
            return _Context.Instructors.Count();
        }

        public int GetTotalCoursesByStatus(string status)
        {
            return _Context.Courses.Count(c => c.status == status);
        }
        public List<Courses> GetallpendingCourses()
        {
            return _Context.Courses
                .Where(c => c.status == "Archive")
                .ToList();
        }

        public List<Courses> GetallActiveCourses()
        {
            return _Context.Courses
                .Where(c => c.status == "Active")
                .ToList();
        }

        public AdminCourseControlVM GetCourseControl()
        {
            var active = _Context.Courses
                .Where(c => c.status == "Active")
                .Include(c => c.Instructor)
                .Include(c => c.Student_Courses)
                .Select(c => new CourseInfoVM
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    Instructor_fname = c.Instructor.User.fname,
                    Instructor_lname = c.Instructor.User.lastName,
                    student_num = c.Student_Courses.Count(),
                    price = c.price,
                    status = c.status
                }).ToList();

            var pending = _Context.Courses
                .Where(c => c.status == "Archive")
                .Include(c => c.Instructor)
                .Include(c => c.Student_Courses)
                .Select(c => new CourseInfoVM
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    Instructor_fname = c.Instructor.User.fname, 
                    Instructor_lname = c.Instructor.User.lastName, 
                    student_num = c.Student_Courses.Count(),
                    price = c.price,
                    status = c.status
                }).ToList();

            return new AdminCourseControlVM
            {
                active_course = active,
                pending_course = pending
            };
        }
        public void ChangeCourseStatus(int courseId, string newStatus)
        {
            var course = _Context.Courses.FirstOrDefault(c => c.CourseId == courseId);

            if (course == null) return;

            course.status = newStatus;
            _Context.SaveChanges();
        }

        public int GetTotalCoursesPending()
        {
            return _Context.Courses.Count(c => c.status == "Archive");
        }

        public decimal GetTotalPayments()
        {
            return _Context.Payments.Sum(p => p.amountPayment);
        }
        public void DeleteCourse(int courseId)
        {
            var course = _Context.Courses
                         .Include(c => c.Student_Courses) // لو عايز تمسح أي ارتباطات بالطلاب
                         .FirstOrDefault(c => c.CourseId == courseId);

            if (course != null)
            {
                // مسح الكورسات المرتبطة بالطلاب أولًا لو محتاج
                if (course.Student_Courses != null && course.Student_Courses.Any())
                {
                    _Context.Student_Courses.RemoveRange(course.Student_Courses);
                }

                _Context.Courses.Remove(course);
                _Context.SaveChanges();
            }
        }

    }
}
