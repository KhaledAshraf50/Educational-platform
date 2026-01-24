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
            // ================== 1️⃣ EXAMS ==================

            // كل الامتحانات اللي الطالب دخلها
            var studentExamStats = _Context.StudentStatistics
                                           .Where(s => s.StudentID == studentId && s.ExamId != null)
                                           .ToList();

            // مجموع درجات الطالب في الامتحانات
            double totalStudentDegrees = studentExamStats.Sum(s => s.degree);

            // IDs الامتحانات اللي دخلها
            var examIds = studentExamStats
                .Select(s => s.ExamId.Value)
                .Distinct()
                .ToList();

            if (examIds == null || examIds.Count == 0)
            {
                return new StudentProgressVM
                {
                    ExamProgress = 0,
                    TaskProgress = 0,
                    OverallProgress = 0
                };
            }

            // مجموع الدرجات النهائية للامتحانات
            var exams = _Context.Exams
                                .Where(e => examIds.Contains(e.ExamID))
                                .ToList();

            double totalExamDegrees = exams.Sum(e => e.degreeExam);

            double examProgress = 0;
            if (totalExamDegrees > 0)
            {
                examProgress = (totalStudentDegrees / totalExamDegrees) * 100.0;
            }

            // ================== 2️⃣ TASKS ==================

            // جدول الأساس للتسكات
            var allTasks = _Context.Studentstaistics_In_Tasks
                .Where(s => s.StudentID == studentId && s.TaskId != null)
                .ToList();

            // مجموع درجات الطالب في التسكات
            double totalStudentDegreesinTask = allTasks.Sum(s => s.degree);

            // IDs التسكات اللي الطالب ليه درجات فيها
            var taskIds = allTasks
                .Select(s => s.TaskId.Value)
                .Distinct()
                .ToList();

            // مجموع الدرجات النهائية للتسكات
            double totalTaskDegrees = 0;
            if (taskIds.Any())
            {
                totalTaskDegrees = _Context.Tasks
                    .Where(t => taskIds.Contains(t.TaskID))
                    .Sum(t => t.NumOfQuestions); // اسم العمود زي ما هو عندك
            }

            double taskProgress = 0;
            if (totalTaskDegrees > 0)
            {
                taskProgress = (totalStudentDegreesinTask / totalTaskDegrees) * 100.0;
            }

            // ================== 3️⃣ OVERALL ==================
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

        public List<showallStudent> showparent()
        {
            var parents = _Context.Parents
                 .Include(s => s.Students)
                 .Include(s => s.User)
                 .Where(s => s.User.status == "Active")
                 .Select(s => new showallStudent
                 {
                     userid = s.User.Id,
                     parentid = s.ID,
                     FullName = s.User.fname + " " + s.User.lastName,
                     Email = s.User.Email,
                     PhoneNumber = s.User.PhoneNumber,
                     childernCount = s.Students.Count(),
                     createat = s.User.CreatedAt ?? DateOnly.FromDateTime(DateTime.Today)
                 }

                 )
                 .ToList();

            return parents;
        }

        public void deleteparent(int userid)
        {
            var parent = _Context.Parents.FirstOrDefault(e => e.UserId == userid);
            _Context.Parents.Remove(parent);
            var user = _Context.Users.FirstOrDefault(e => e.Id == userid);
            _Context.Users.Remove(user);

            _Context.SaveChanges();
        }
    }
}
