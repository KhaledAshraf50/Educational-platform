using Luno_platform.Models;
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

            if (student == null)
            {
                throw new Exception("Student not found with ID = " + id);
            }

            return student;

        }

        public List<Courses> GetStudentCourses(int id)
        {

            return _Context.Student_Courses.Include(Courses => Courses.Course)
                .Where(student => student.StudentId == id)
                .Select(sc => sc.Course)
                .ToList();
        }
    }


    }
