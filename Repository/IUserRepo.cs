using Luno_platform.Models;

namespace Luno_platform.Repository
{
    public interface IUserRepo
    {
        Users GetById(int id);
        void Update(Users user);
        void Save();
    }

}
