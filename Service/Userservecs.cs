using Luno_platform.Models;
using Luno_platform.Repository;
using Luno_platform.Viewmodel;

namespace Luno_platform.Service
{
    public class Userservecs : BaseService<Users>, IUserservecs
    {

        public  IUserRepo _repeRepo;

        public Userservecs(IUserRepo userRepo) : base((I_BaseRepository<Users>)userRepo)
        {
            _repeRepo = userRepo;
        }

        public string deleteuser(int userid)
        {
            return _repeRepo.deleteuser(userid);
        }

        public showadetailsusers GetUserDetails(int userId)
        {
            return _repeRepo.GetUserDetails(userId);
        }

        public List<showallStudent> NotActiveusers()
        {
            return _repeRepo.NotActiveusers();
        }

        public List<showallStudent> pandingusers()
        {
            return _repeRepo.pandingusers() ;
        }

        public void SetUsersActive(int userid)
        {
            _repeRepo.SetUsersActive(userid);
        }
    }
}
