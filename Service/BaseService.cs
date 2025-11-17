
using Luno_platform.Repository;

namespace Luno_platform.Service
{
    public class BaseService<T> : I_BaseService<T> where T : class
    {

        public I_BaseRepository<T> Repo;
        public BaseService( I_BaseRepository<T> Repository)
        {
            Repo = Repository;
            
        }

        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }


}
