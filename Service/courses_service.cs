using Luno_platform.Models;
using Luno_platform.Repository;

namespace Luno_platform.Service
{
    public class courses_service : BaseService<Courses>, Icourses_service
    {
        private readonly Icourses_repo _coursesRepo;

        public courses_service(Icourses_repo coursesRepo) : base(coursesRepo)
        {
            _coursesRepo = coursesRepo;
        }
        public Courses Infocourse(int courseid)
        {
            return _coursesRepo.Infocourse(courseid);
        }

        public List<Courses> showAllcoursebyclassandinstructor(int instructorid, int classid)
        {
            return _coursesRepo.showAllcoursebyclassandinstructor(instructorid, classid);
        }
    }
}
