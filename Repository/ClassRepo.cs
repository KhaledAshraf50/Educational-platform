using Luno_platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Luno_platform.Repository
{
    public class ClassRepo :BaseRepository<Classes>, IClassRepo
    {
        public ClassRepo(LunoDBContext database) : base(database)
        {
        }

        public List<Classes> GetAll()
        {
            return _Context.Classes.ToList();
        }

        public Classes GetById(int id)
        {
            return _Context.Classes.FirstOrDefault(c => c.ClassID == id);
        }
    }
}
