using Luno_platform.Models;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Mvc;

namespace Luno_platform.Repository
{
    public interface IUserRepo
    {
        Users GetById(int id);
        void Update(Users user);
        void Save();

        List<showallStudent> pandingusers();

        void SetUsersActive(int userid);

         List<showallStudent> NotActiveusers();
        public string deleteuser(int userid);

        public showadetailsusers GetUserDetails(int userId);

    }

}
