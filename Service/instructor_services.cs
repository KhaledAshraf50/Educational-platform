using Luno_platform.Models;
using Luno_platform.Repository;
using Luno_platform.Viewmodel;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Service
{
    public class instructor_services : BaseService<Instructor> ,I_instructor_services 
    {
        private readonly I_instructor_repo _instorRepo;

        public instructor_services(I_instructor_repo _instructor):base(_instructor) {
        
            _instorRepo = _instructor;
            

        }

        public void deleteinstructor(int userid)
        {
            _instorRepo.deleteinstructor(userid);
        }

        public IEnumerable<Instructor> GetAll_Instructors_With_User_with_subject()
        {
         return _instorRepo.GetAll_Instructors_With_User_with_subject();
        }

        public List<Instructor> getprotfolioteacher(int id)
        {
            return _instorRepo.getprotfolioteacher(id);
        }

        public List<Instructor> infoinstructors()
        {
            return _instorRepo.infoinstructors();
        }

        public List<showallStudent> showinstructor()
        {
            return _instorRepo.showinstructor();
        }
    }
}
