using Luno_platform.Repository;

namespace Luno_platform.Service
{
    public class BaseService<T> : I_BaseService<T> where T : class
    {
        public I_BaseRepository<T> Repo;

        public BaseService(I_BaseRepository<T> repository)
        {
            Repo = repository;
        }

        public void Add(T entity)
        {
            Repo.Add(entity);
        }

        public void Delete(T entity)
        {
            Repo.Delete(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return Repo.GetAll();
        }

        public T GetById(int id)
        {
            return Repo.GetById(id); 
        }

        public void Save()
        {
            Repo.Save();
        }

        public void Update(T entity)
        {
            Repo.Update(entity);
        }
    }
}
