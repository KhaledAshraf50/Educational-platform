
using Luno_platform.Models;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Repository
{
    public class BaseRepository<T> : I_BaseRepository<T> where T : class
    {

        public LunoDBContext _Context;
        public DbSet<T> Table;

        public BaseRepository( LunoDBContext lunoDBContext)
        {
            _Context = lunoDBContext;
            Table = _Context.Set<T>();
        }
        public void Add(T entity)
        {
            Table.Add(entity);
        }
        public void Delete(T entity)
        {
            var en = Table.Find(entity);
            if (en != null)
            {
                Table.Remove(en);
            }
        }
        //public IEnumerable<T> GetAll()
        //{
        //    throw new NotImplementedException();
        //}
        public List<T> GetAll()
        {
            return Table.ToList();
        }
        public T GetById(int id)
        {
            return Table.Find(id);
        }
        public void Save()
        {
          _Context.SaveChanges();
        }
        public void Update(T entity)
        {
            Table.Update(entity);
        }
    }
}
