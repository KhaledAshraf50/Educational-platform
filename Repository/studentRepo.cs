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

        //public int? GetStudentIdByUserId(int userId)
        //{
        //    var student = _Context.Students
        //        .Include(s => s.User)
        //        .FirstOrDefault(s => s.User.Id == userId);

        //    return student?.StudentID;   // لو مش طالب هترجع null
        //}

        public Student GetStudent(int userId)
        {
            //int? studentId = GetStudentIdByUserId(userId);
            var student = _Context.Students
                .Include(s => s.User)
                .Include(s => s.Classes)
                .Where(s => s.User.Id == userId)  // البحث حسب User.Id
                .Select(s => new Student
                {
                    StudentID = s.StudentID,

                    User = new Users
                    {
                        Id = s.User.Id,
                        fname = s.User.fname,
                        lastName = s.User.lastName,
                        Image = s.User.Image

                    },
                    Classes = new Classes
                    {
                        ClassID = s.Classes.ClassID,
                        ClassName = s.Classes.ClassName
                    }
                })
                .FirstOrDefault(); // هترجع أول طالب يطابق UserId

            return student;
        }
        public Student GetStudentt(int userId)
        {
            return _Context.Students.Include(u => u.User).FirstOrDefault(u => u.UserId == userId);
        }
        public Student GetByUserId(int userId)
        {
            return _Context.Students
                           .Include(s => s.Classes) // ⬅️ مهم
                           .FirstOrDefault(s => s.UserId == userId);
        }

        public List<Courses> GetStudentCourses(int userId)
        {
            int? studentId = GetStudentIdByUserId(userId);
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
         status = sc.Course.status,
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
        public List<Courses> GetStudentCourses(int userId, int page = 1, int pageSize = 10)
        {
            int? studentId = GetStudentIdByUserId(userId);
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


        public List<StudentCourseFullDataVM> GetStudentCoursesFullData(int userId)
        {
            int? studentId = GetStudentIdByUserId(userId);
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
                                .FirstOrDefault(),
                    Degree_task=_Context.Studentstaistics_In_Tasks
                                .Where(ss => ss.StudentID == x.sc.StudentId && ss.TaskId == x.cc.taskId)
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
            result.ForEach(r =>
            {
                if (string.IsNullOrEmpty(r.Degree_task))
                    r.Degree_task = "لا يوجد بيانات";
            });

            return result;
        }
        public List<Payments> GetPayments(int userId)
        {
            int? studentId = GetStudentIdByUserId(userId);
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
        public void AddStudentCourse(Student_Courses studentCourse)
        {
            _Context.Student_Courses.Add(studentCourse);
            _Context.SaveChanges();
        }

        public int getStudentId(int userid)
        {
            var student = _Context.Students
                .Include(s => s.User)
                .FirstOrDefault(s => s.User.Id == userid);

            if (student == null)
            {
                return -1;
            }

            return student.StudentID;
        }

        public bool isSubdcrip(int studentid, int courseid)
        {
            var sub = _Context.Student_Courses.FirstOrDefault(s => s.StudentId == studentid && s.CourseId == courseid);

            if (sub ==null)
            {
                return false;

            }
            return true;
        }

        public List<showallStudent> showStudents()
        {
            var students = _Context.Students
                .Include(s => s.Student_Courses)
                .Include(s => s.User)
                .Where(s=> s.User.status == "Active")
                .Select(s => new showallStudent { 
                    userid = s.User.Id,
                    StudentID = s.StudentID,
                    FullName = s.User.fname + " " + s.User.lastName,
                    Email = s.User.Email,
                    PhoneNumber = s.User.PhoneNumber,
                    CoursesCount = s.Student_Courses.Count(),
                    createat = s.User.CreatedAt ?? DateOnly.FromDateTime(DateTime.Today)
                }
               
                )
                .ToList();

            return students;
        }

        public void DeleteStudent(int userid)
        {
            var student = _Context.Students.FirstOrDefault(e => e.UserId == userid);
            _Context.Students.Remove(student);
            var user = _Context.Users.FirstOrDefault(e => e.Id == userid);
            _Context.Users.Remove(user);
            _Context.SaveChanges();
        }

        public void SetUserPending(int userid)
        {
            var user = _Context.Users.FirstOrDefault(e => e.Id == userid);
            if (user != null)
            {
                user.status = "NotActive";
                _Context.SaveChanges();
            }

        }
    }
}