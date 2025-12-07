using Luno_platform.Models;
using Luno_platform.Viewmodel;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Repository
{
    public class instructor_repo : BaseRepository<Instructor> , I_instructor_repo
    {
        public instructor_repo(LunoDBContext database) : base(database)
        {
        }

        public void deleteinstructor(int userid)
        {
            var instructor = _Context.Instructors.FirstOrDefault(e => e.UserId == userid);
            _Context.Instructors.Remove(instructor);
            var user = _Context.Users.FirstOrDefault(e => e.Id == userid);
            _Context.Users.Remove(user);

            _Context.SaveChanges();
        }

        public IEnumerable<Instructor> GetAll_Instructors_With_User_with_subject()
        {
           return _Context.Instructors.Include(e=> e.User).Include(r=> r.Subject).ToList();
        }

        public List<Instructor> getprotfolioteacher(int id)
        {
            return _Context.Instructors
                          .Include(i => i.User)
                          .Include(i => i.Subject)
                          .Include(i => i.instructor_classescs)
                              .ThenInclude(ic => ic.classes)
                          .Where(i=> i.instructorID==id)
                        
                          .Select(i => new Instructor
                          {
                              instructorID = i.instructorID,
                              //Image = i.Image,
                              motto = i.motto,
                              bio = i.bio,
                              eligible = i.eligible,

                              User = new Users
                              {
                                  fname = i.User.fname,
                                  lastName = i.User.lastName,
                                  Email=i.User.Email,
                                  PhoneNumber=i.User.PhoneNumber,
                                  Image = i.User.Image


                              },


                              Subject = new Subject
                              {
                                  SubjectNameAR = i.Subject.SubjectNameAR
                              },


                              instructor_classescs = i.instructor_classescs
                                                     .Select(ic => new instructor_classescs
                                                     {
                                                         classId = ic.classId,
                                                         classes = new Classes
                                                         {
                                                             ClassName = ic.classes.ClassName

                                                         }
                                                     })
                                                     .ToList()
                          })
                          .ToList();
        }

        public List<Instructor> infoinstructors()
        {
           return _Context.Instructors
               .Include(i => i.User)
               .Include(i => i.Subject)
               .Include(i => i.instructor_classescs)
                   .ThenInclude(ic => ic.classes)
               .Select(i => new Instructor
               {
                   instructorID = i.instructorID,
                   //Image=i.Image,
                   motto=i.motto,
                   bio=i.bio,
                   eligible=i.eligible,
                   // بيانات المستخدم
                   User = new Users
                   {
                       fname = i.User.fname,
                       lastName = i.User.lastName,
                       Image=i.User.Image

                   },

                   // المادة
                   Subject = new Subject
                   {
                       SubjectNameAR = i.Subject.SubjectNameAR
                   },

                   // الفصول
                   instructor_classescs = i.instructor_classescs
                                          .Select(ic => new instructor_classescs
                                          {
                                              classId = ic.classId,
                                              classes = new Classes
                                              {
                                                  ClassName = ic.classes.ClassName
                                              }
                                          })
                                          .ToList()
               })
               .ToList();
        }

        public List<showallStudent> showinstructor()
        {
            var students = _Context.Instructors
                 .Include(s => s.courses)
                 .Include(s => s.User)
                 .Where(s => s.User.status == "Active")
                 .Select(s => new showallStudent
                 {
                     userid = s.User.Id,
                     instructorid = s.instructorID,
                     FullName = s.User.fname + " " + s.User.lastName,
                     Email = s.User.Email,
                     PhoneNumber = s.User.PhoneNumber,
                     CoursesCount = s.courses.Count(),
                     createat = s.User.CreatedAt ?? DateOnly.FromDateTime(DateTime.Today)
                 }

                 )
                 .ToList();

            return students;
        }
    }
}
