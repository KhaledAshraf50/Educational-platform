using Luno_platform.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Luno_platform.Repository
{
    public class courses_repo: BaseRepository<Courses>, Icourses_repo
    {
        public courses_repo(LunoDBContext database) : base(database)
        {
        }

        //public Courses Infocourse(int courseid)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Courses> showAllcoursebyclassandinstructor(int instructorid, int classid)
        //{
        //    throw new NotImplementedException();
        //}

        public List<Subject> GetAllSubjects()
        {
            return _Context.Subjects.ToList();   
        }
        public List<Classes> GetAllClasses()
        {
           
            return _Context.Classes.ToList();
        }
        public Courses Infocourse(int courseId)
        {
            return _Context.Courses
                .Include(c => c.Instructor).ThenInclude(i => i.User)
                .Include(c => c.CourseContent).ThenInclude(cc => cc.Exams)
                .Include(c => c.CourseContent).ThenInclude(cc => cc.Tasks)
                .Where(c => c.CourseId == courseId)
                .Select(c => new Courses
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    description = c.description,
                    createdAt = c.createdAt,
                    price = c.price,
                    Image = c.Image,
                    instructorID = c.instructorID,

                    // Instructor Info
                    Instructor = new Instructor
                    {
                        instructorID = c.Instructor.instructorID,
                        User = new Users
                        {
                            fname = c.Instructor.User.fname,
                            lastName = c.Instructor.User.lastName
                        }
                    },

                    // Course Content
                    CourseContent = c.CourseContent == null ? null : new CourseContent
                    {
                        Id = c.CourseContent.Id,
                        nameurl1=c.CourseContent.nameurl1,
                        nameurl2 = c.CourseContent.nameurl2,

                        nameurl3 = c.CourseContent.nameurl3,

                        Url1 = c.CourseContent.Url1,
                        Url2 = c.CourseContent.Url2,
                        Url3 = c.CourseContent.Url3,

                        // Exam
                        Exams = c.CourseContent.Exams == null ? null : new Exams
                        {
                            ExamID = c.CourseContent.Exams.ExamID,
                            ExamName = c.CourseContent.Exams.ExamName
                        },

                        // Task
                        Tasks = c.CourseContent.Tasks == null ? null : new Tasks
                        {
                            TaskID = c.CourseContent.Tasks.TaskID,
                            TaskName = c.CourseContent.Tasks.TaskName
                        }
                    }
                })
                .FirstOrDefault();
        }

        public List<Courses> showAllcoursebyclassandinstructor(int instructorid, int classid)
        {
            return _Context.Courses
                
                .Where(e => e.classID == classid && e.instructorID == instructorid)

                          .Select(i => new Courses
                          {
                              CourseId = i.CourseId,
                              CourseName = i.CourseName,
                              Image = i.Image,
                              description = i.description,
                              createdAt = i.createdAt,
                              price = i.price,


                             


                          })


                .ToList();
        }
    }


}
