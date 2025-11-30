using Luno_platform.Models;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Repository
{
    public class UserRepo : BaseRepository<Users>, IUserRepo
    {
        public UserRepo(LunoDBContext database) : base(database)
        {
        }
        public Users GetById(int id)
        {
            return _Context.Users.FirstOrDefault(x => x.Id == id);
        }

        public void Update(Users user)
        {
            _Context.Users.Update(user);
        }

        public void Save()
        {
            _Context.SaveChanges();
        }

    }
}
