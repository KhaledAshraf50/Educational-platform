using Luno_platform.Models;

namespace Luno_platform.Service
{
    public interface I_instructor_services :I_BaseService<Instructor>
    {
        IEnumerable<Instructor> GetAll_Instructors_With_User_with_subject();
        List<Instructor> infoinstructors();

        List<Instructor> getprotfolioteacher( int id);

    }
}
