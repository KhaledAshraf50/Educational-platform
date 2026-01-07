using Luno_platform.Models;
using Luno_platform.Viewmodel;

namespace Luno_platform.Service
{
    public interface Icourses_service: I_BaseService<Courses>
    {
        CourseFullInfoVM Infocourse(int courseid);
        List<Courses> showAllcoursebyclassandinstructor(int instructorid, int classid);
        List<Classes> GetAllClasses();
        List<Subject> GetAllSubjects();
        List<Courses> showallcourses();

        List<Courses> GetTopCoursesThisWeek();



    }
}
