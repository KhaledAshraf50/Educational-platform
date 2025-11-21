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

        public Courses Infocourse(int courseid)
        {
            throw new NotImplementedException();
        }



        //public Courses Infocourse(int courseid)
        //{
        //    return _Context.Courses
        //        .Include(i => i.Instructor).ThenInclude(i => i.User)
        //        .Include(c => c.CourseContent)
        //            .ThenInclude(cc => cc.Task_Contents).ThenInclude(c => c.Tasks)
        //        .Include(c => c.CourseContent)
        //            .ThenInclude(cc => cc.Exams_Content).ThenInclude(c => c.Exams)
        //        .Where(c => c.Courseid == courseid)
        //        .Select(i => new Courses
        //        {
        //            CourseName = i.CourseName,
        //            Image = i.Image,
        //            description = i.description,
        //            createdAt = i.createdAt,
        //            price = i.price,
        //            instructorID=i.instructorID,


        //            Instructor = new Instructor
        //            {
        //                instructorID = i.Instructor.instructorID,

        //                User = new Users
        //                {
        //                    fname = i.Instructor.User.fname,
        //                    lastName = i.Instructor.User.lastName
        //                }


        //            },
        //            CourseContent = new CourseContent
        //            {
        //                Id = i.CourseContent.Id,
        //                Url1 = i.CourseContent.Url1,
        //                Url2 = i.CourseContent.Url2,
        //                Url3 = i.CourseContent.Url3,

        //                Exams_Content = i.CourseContent.Exams_Content
        //                    .Select(e => new Exams_contentcs
        //                    {
        //                        ExamId = e.ExamId,

        //                        Exams = new Exams { 
        //                            ExamName=e.Exams.ExamName,
        //                                          }




        //                    })
        //                    .ToList(),

        //                Task_Contents = i.CourseContent.Task_Contents
        //                    .Select(t => new Task_content
        //                    {
        //                        TaskId = t.TaskId,


        //                        Tasks = new Tasks{ 

        //                            TaskName=t.Tasks.TaskName

        //                        }



        //                    })
        //                    .ToList()
        //            }
        //        })
        //        .FirstOrDefault();   
        //}

        public List<Courses> showAllcoursebyclassandinstructor(int instructorid, int classid)
        {
            return _Context.Courses
                .Include(c => c.CourseContent)
                .Where(e => e.classID == classid && e.instructorID == instructorid)

                          .Select(i => new Courses
                          {
                              Courseid=i.Courseid,
                              CourseName = i.CourseName,
                              Image = i.Image,
                              description = i.description,
                              createdAt = i.createdAt,
                              price = i.price,


                              CourseContent = new CourseContent
                              {
                                  Id = i.CourseContent.Id


                              }


                          })

                          
                .ToList();
        }
    }
    
    
}
