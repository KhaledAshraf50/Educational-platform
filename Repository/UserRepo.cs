using Luno_platform.Models;
using Luno_platform.Viewmodel;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Repository
{
    public class UserRepo : BaseRepository<Users>, IUserRepo
    {
        public UserRepo(LunoDBContext database) : base(database)
        {
        }
        public Users GetById(int id)
        {
            return _Context.Users.FirstOrDefault(x => x.Id == id);
        }

        public void Update(Users user)
        {
            _Context.Users.Update(user);
        }

        public void Save()
        {
            _Context.SaveChanges();
        }

        public List<showallStudent> pandingusers()
        {
            var result = _Context.Users
                .Include(s => s.student)
                .Include(s => s.parent)
                .Include(s => s.Instructor)
                .Where(s => s.status == "Pending")
                 .Select(s => new showallStudent
                 {
                     userid = s.Id,
                     StudentID = s.student.StudentID,
                     parentid=s.parent.ID,
                     instructorid=s.Instructor.instructorID,
                     FullName = s.fname + " " + s.lastName,
                     Email = s.Email,
                     PhoneNumber = s.PhoneNumber,
                     role =s.role,

                     createat = s.CreatedAt ?? DateOnly.FromDateTime(DateTime.Today)
                 });

            return result.ToList();

        }

        public void SetUsersActive(int userid)
        {
            var user = _Context.Users.FirstOrDefault(e => e.Id == userid);
            if (user != null)
            {
                user.status = "Active";
                _Context.SaveChanges();
            }
        }

        public string deleteuser(int userid)
        {
            var user = _Context.Users.FirstOrDefault(u => u.Id == userid);

            if (user == null)
                return "المستخدم غير موجود.";

            switch (user.role)
            {
                case "student":
                    var student = _Context.Students.FirstOrDefault(s => s.UserId == userid);

                    if (student != null)
                    {
                        bool hasPayments = _Context.Payments.Any(p => p.StudentID == student.StudentID);

                        if (hasPayments)
                        {
                            return "⚠️ لا يمكن حذف هذا الطالب لأنه مسجل في عمليات دفع.";
                        }

                        _Context.Students.Remove(student);
                    }
                    break;

                case "instructor":
                    var teacher = _Context.Instructors.FirstOrDefault(t => t.UserId == userid);
                    if (teacher != null)
                        _Context.Instructors.Remove(teacher);
                    break;

                case "admin":
                    var admin = _Context.Admins.FirstOrDefault(a => a.UserId == userid);
                    if (admin != null)
                        _Context.Admins.Remove(admin);
                    break;
            }

            _Context.Users.Remove(user);
            _Context.SaveChanges();

            return "تم حذف المستخدم بنجاح.";
        }


    }
}
