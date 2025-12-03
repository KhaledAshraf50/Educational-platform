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

        public string deleteuser(int userid);

    }

}
