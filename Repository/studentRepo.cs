using Luno_platform.Models;
using Luno_platform.Viewmodel;
using Microsoft.EntityFrameworkCore;
namespace Luno_platform.Repository
{
    public class studentRepo : BaseRepository<Student>, IstudentRepo
    {
        public studentRepo(LunoDBContext database) : base(database)
        {
        }

        public Student GetStudent(int id)
        {
            var student = _Context.Students
     .Include(s => s.User)
     .Include(s => s.Classes)
     .Select(s => new Student
     {
         StudentID = s.StudentID,
         Image = s.Image
         ,
         User = new Users
         {
             ID = s.User.ID,
             fname = s.User.fname,
             lastName = s.User.lastName
         },
         Classes = new Classes
         {
             ClassID = s.Classes.ClassID,
             ClassName = s.Classes.ClassName
         }

     }
     )
     .FirstOrDefault(s => s.StudentID == id);



            return student;

        }

        public List<Courses> GetStudentCourses(int studentId)
        {
            var courses = _Context.Student_Courses
     .Where(sc => sc.StudentId == studentId)
     .Include(sc => sc.Course)
     .ThenInclude(c => c.Subjects)
     .Select(sc => new Courses
     {
         CourseId = sc.Course.CourseId,
         CourseName = sc.Course.CourseName ?? "بدون اسم",
         description = sc.Course.description ?? "لا يوجد وصف",
         price = sc.Course.price,
         Image = sc.Course.Image ?? "default-image.png",
         createdAt = sc.Course.createdAt,
         instructorID = sc.Course.instructorID,
         SubjectId = sc.Course.SubjectId,
         Subjects = new Subject
         {
             SubjectID = sc.Course.Subjects.SubjectID,
             SubjectNameAR = sc.Course.Subjects.SubjectNameAR ?? "اسم المادة غير متوفر",
             SubjectNameEN = sc.Course.Subjects.SubjectNameEN ?? "Subject name not available"
         }
     })
     .ToList();


            return courses;
        }
        public List<Courses> GetStudentCourses(int studentId, int page = 1, int pageSize = 10)
        {
            var coursesQuery = _Context.Student_Courses
                .Where(sc => sc.StudentId == studentId)
                .Include(sc => sc.Course)            // Include قبل الـ Select
                    .ThenInclude(c => c.Subjects)
                .OrderBy(sc => sc.Course.CourseId)  // ترتيب ثابت

                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(sc => new Courses
                {
                    CourseId = sc.Course.CourseId,
                    CourseName = sc.Course.CourseName ?? "بدون اسم",
                    description = sc.Course.description ?? "لا يوجد وصف",
                    price = sc.Course.price,
                    Image = sc.Course.Image ?? "default-image.png",
                    createdAt = sc.Course.createdAt,
                    instructorID = sc.Course.instructorID,
                    SubjectId = sc.Course.SubjectId,
                    Subjects = sc.Course.Subjects != null ? new Subject
                    {
                        SubjectID = sc.Course.Subjects.SubjectID,
                        SubjectNameAR = sc.Course.Subjects.SubjectNameAR ?? "لا يوجد مادة",
                        SubjectNameEN = sc.Course.Subjects.SubjectNameEN ?? "Subject name not available"
                    } : null
                });

            return coursesQuery.ToList();
        }

        //public List<StudentCourseFullDataVM> GetStudentCoursesFullData(int studentId)
        //{
        //    return _Context.Student_Courses
        //        .Where(sc => sc.StudentId == studentId)
        //        .Include(sc => sc.Course)
        //            .ThenInclude(c => c.Subjects)
        //        .Include(sc => sc.Student)
        //            .ThenInclude(st => st.StudentStatistics)
        //        .Select(sc => new StudentCourseFullDataVM
        //        {
        //            CourseId = sc.Course.CourseId,
        //            CourseName = sc.Course.CourseName,
        //            CourseDescription = sc.Course.description,
        //            SubjectNameAR = sc.Course.Subjects.SubjectNameAR,
        //            SubjectNameEN = sc.Course.Subjects.SubjectNameEN,
        //            Degree = sc.cou.Student..StudentStatistics

        //                        .FirstOrDefault(ss => ss.StudentID == sc.StudentId)
        //                        .degree
        //        })
        //        .ToList();
        //}
        //public List<StudentCourseFullDataVM> GetStudentCoursesFullData(int studentId)
        //{
        //    return _Context.Student_Courses
        //        .Where(sc => sc.StudentId == studentId)
        //        .Include(sc => sc.Course)
        //            .ThenInclude(c => c.Subjects)
        //        .Include(sc => sc.Student)
        //            .ThenInclude(st => st.StudentStatistics)
        //        .Include(sc => sc.Course)
        //            .ThenInclude(c => c.CourseContent)
        //        .Select(sc => new StudentCourseFullDataVM
        //        {
        //            CourseId = sc.Course.CourseId,
        //            CourseName = sc.Course.CourseName,
        //            CourseDescription = sc.Course.description,
        //            SubjectNameAR = sc.Course.Subjects.SubjectNameAR,
        //            SubjectNameEN = sc.Course.Subjects.SubjectNameEN,

        //            // جلب أول درجة مرتبطة بالامتحان الخاص بالكورس
        //            Degree = sc.Student.StudentStatistics
        //                        .Where(ss => ss.ExamId == sc.Course.CourseContent.ExamId)
        //                        .Select(ss => (decimal?)ss.degree)
        //                        .FirstOrDefault()
        //        })
        //        .ToList();
        //}
        public List<StudentCourseFullDataVM> GetStudentCoursesFullData(int studentId)
        {
            var query =
                from sc in _Context.Student_Courses
                where sc.StudentId == studentId
                // left join مع CourseContents على cousrsid
                join cc in _Context.CourseContents
                    on sc.Course.CourseId equals cc.cousrsid into ccGroup
                from cc in ccGroup.DefaultIfEmpty()   // cc ممكن يبقى null (مافيش محتوى)
                select new
                {
                    sc,
                    cc
                };

            var projected = query
                .Select(x => new StudentCourseFullDataVM
                {
                    CourseId = x.sc.Course.CourseId,
                    CourseName = x.sc.Course.CourseName,
                    CourseDescription = x.sc.Course.description,
                    SubjectNameAR = x.sc.Course.Subjects.SubjectNameAR,
                    SubjectNameEN = x.sc.Course.Subjects.SubjectNameEN,

                    // Subquery: جلب الدرجة من StudentStatistics حيث StudentID و ExamId يطابقوا
                    Degree = _Context.StudentStatistics
                                .Where(ss => ss.StudentID == x.sc.StudentId && ss.ExamId == x.cc.ExamId)
                                .Select(ss => ss.degree.ToString())
                                .FirstOrDefault()
                });

            // ممكن يحصل تكرار للكورس لو ليه أكثر من CourseContent -> نضم النتائج بحسب CourseId ونأخذ أول صف لكل كورس
            var result = projected
                .AsEnumerable()                       // نجلب النتائج للذاكرة مؤقتاً علشان نقدر نعمل GroupBy بسهولة
                .GroupBy(p => p.CourseId)
                .Select(g => g.First())
                .ToList();

            // لو عايز Degree بدل null يظهر "لا يوجد بيانات"
            result.ForEach(r =>
            {
                if (string.IsNullOrEmpty(r.Degree))
                    r.Degree = "لا يوجد بيانات";
            });

            return result;
        }
        public List<Payments> GetPayments(int studentId)
        {
            var payments = _Context.Payments
         .Where(p => p.StudentID == studentId)
         .Include(p => p.Courses)
         .Select(p => new Payments
         {
             ID = p.ID,
             date = p.date,
             status = p.status,
             amountPayment = p.amountPayment,
             courseId = p.courseId,
             Courses = new Courses
             {
                 CourseId = p.Courses.CourseId,
                 CourseName = p.Courses.CourseName ?? "بدون اسم"
             }
         })
         .ToList();



            return payments;




        }


    }
}
