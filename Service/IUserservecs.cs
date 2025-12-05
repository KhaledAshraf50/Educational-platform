using Luno_platform.Models;
using Luno_platform.Viewmodel;

namespace Luno_platform.Service
{
    public interface IUserservecs: I_BaseService<Users>
    {
        List<showallStudent> pandingusers();

        void SetUsersActive(int userid);

         public string deleteuser(int userid);
        List<showallStudent> NotActiveusers();
        public showadetailsusers GetUserDetails(int userId);

    }
}
