using Luno_platform.Models;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Repository
{
    public class ParentRepo : BaseRepository<Parent>, IParentRepo
    {
        public ParentRepo(LunoDBContext db) : base(db)
        {

        }

        public List<Student> GetStds(int id)
        {
            return _Context.Students.Include(u => u.User).Include(u => u.Classes).Include(u => u.Student_Courses).Where(p => p.ParentId == id).ToList();
        }
        public List<Classes> GetClasses()
        {
            return _Context.Classes.ToList();
        }
        public Parent GetByUserId(int userId)
        {
            return _Context.Parents.Include(u=>u.User).FirstOrDefault(p => p.UserId == userId);
        }

        public Student GetStudentDetails(int id)
        {
            return _Context.Students.Include(u => u.User).Include(c => c.Classes).FirstOrDefault(s => s.StudentID == id);
        }
        public Student GetStudentByNationalID(string nationalID)
        {
            return _Context.Students.Include(u => u.User).Include(c => c.Classes).FirstOrDefault(s => s.User.nationalID == nationalID);
        }
       public Student GetStudentById(int id)
       {
            return _Context.Students.Find(id);
       }
        public void UpdateStudent(Student student)
        {
            _Context.Students.Update(student);
            _Context.SaveChanges();
        }
        public Parent GetParent(int id)
        {
            var parent = _Context.Parents.Include(u => u.User).FirstOrDefault(p => p.ID == id);
            if (parent == null)
            {
                return null;
            }
            return parent;
        }
        public Parent EditParentSetting(ParentSettingVM pVM)
        {
            var parent = _Context.Parents.Include(u => u.User).FirstOrDefault(p => p.ID == pVM.ParentID);
            if (parent == null)
            {
                throw new Exception("Parent not found");
            }
            return parent;
        }

        public int GetNoOfStudents(int parentId)
        {
            int count = _Context.Students.Count(s => s.ParentId == parentId);
            return count;
        }
        public StudentProgressVM GetStudentProgress(int studentId)
        {
            // 1- كل الامتحانات اللي الطالب دخلها
            var studentExamStats = _Context.StudentStatistics
                                           .Where(s => s.StudentID == studentId && s.ExamId != null)
                                           .ToList();

            // مجموع درجات الطالب
            double totalStudentDegrees = studentExamStats.Sum(s => s.degree);

            // جيب الامتحانات اللي الطالب دخلها
            var examIds = studentExamStats.Select(s => s.ExamId.Value).Distinct().ToList();

            // هات الامتحانات الأصلية علشان نعرف total degree
            var exams = _Context.Exams
                                .Where(e => examIds.Contains(e.ExamID))
                                .ToList();

            // مجموع درجات الامتحانات
            double totalExamDegrees = exams.Sum(e => e.degreeExam);

            double examProgress = 0;
            if (totalExamDegrees > 0)
            {
                examProgress = (totalStudentDegrees / totalExamDegrees) * 100.0;
            }

            // 2- TASK Progress
            var allTasks = _Context.Tasks.ToList();

            // عدد الواجبات اللي الطالب سلّمها (لو StudentAnswer مستخدم لتسجيل التسليم)
            var submittedTasks = _Context.StudentAnswers
                                         .Where(s => s.StudentID == studentId && s.TaskId != null)
                                         .Select(s => s.TaskId)
                                         .Distinct()
                                         .Count();

            double taskProgress = 0;
            if (allTasks.Count > 0)
            {
                taskProgress = (double)submittedTasks / allTasks.Count * 100.0;
            }

            // 3- Overall Progress
            double overall = (examProgress + taskProgress) / 2;

            return new StudentProgressVM
            {
                ExamProgress = Math.Round(examProgress, 2),
                TaskProgress = Math.Round(taskProgress, 2),
                OverallProgress = Math.Round(overall, 2)
            };
        }

        public List<Payments> GetPayments(int studentId)
        {
            var payments = _Context.Payments
         .Where(p => p.StudentID == studentId)
         .Include(p => p.Courses)
         .Include(s=>s.Student)
         .ThenInclude(u=>u.User)
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
             },
             StudentID=p.StudentID,
             Student = new Student
             {
                 StudentID=p.Student.StudentID,
                 User = new Users
                 {
                     fname=p.Student.User.fname,
                     lastName=p.Student.User.lastName
                 }
             }
         })
         .ToList();
            return payments;
        }

        public List<Student> GetStudentBelongToParent(int parentId)
        {
            return _Context.Students.Where(s=>s.ParentId == parentId).ToList();
        }
    }
}
