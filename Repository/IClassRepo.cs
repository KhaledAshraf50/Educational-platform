using Luno_platform.Models;
using System.Collections.Generic;

namespace Luno_platform.Repository
{
    public interface IClassRepo
    {
        List<Classes> GetAll();
        Classes GetById(int id);
    }
}
