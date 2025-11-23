using Luno_platform.Models;

namespace Luno_platform.Repository
{
    public interface Icourses_repo:I_BaseRepository<Courses>
    {

        Courses Infocourse(int courseid);
        List<Courses> showAllcoursebyclassandinstructor(int instructorid, int classid);
      
    }
}
