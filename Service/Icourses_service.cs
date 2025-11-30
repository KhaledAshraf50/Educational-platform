using Luno_platform.Models;

namespace Luno_platform.Service
{
    public interface Icourses_service: I_BaseService<Courses>
    {
        Courses Infocourse(int courseid);
        List<Courses> showAllcoursebyclassandinstructor(int instructorid, int classid);
        List<Classes> GetAllClasses();
        List<Subject> GetAllSubjects();



    }
}
