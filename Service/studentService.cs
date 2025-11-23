using Luno_platform.Models;
using Luno_platform.Repository;
using Luno_platform.Viewmodel;
using Microsoft.EntityFrameworkCore;
namespace Luno_platform.Service
{
    public class studentService : BaseService<Student>, IstudentService
    {
        private IstudentRepo _repository;

        public studentService(IstudentRepo repository) : base(repository)
        {
            _repository = repository;
        }
        public Student GetStudent(int id)
        {
           
            return _repository.GetStudent(id);
        }
        public List<Courses> GetStudentCourses(int id)
        {
            return _repository.GetStudentCourses(id);
        }
       public List<StudentCourseFullDataVM> GetStudentCoursesFullData(int id)
        {
            return _repository.GetStudentCoursesFullData(id);
        }
    }
    
}
