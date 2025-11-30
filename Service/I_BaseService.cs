namespace Luno_platform.Service
{
    public interface I_BaseService<T> where T : class

    {

        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();

        int? GetStudentIdByUserId(int userId);
        public int? GetParentIdByUserId(int userId);
        public int? GetInstructorIdByUserId(int userId);
        bool HasStudentTakenExam(int studentId, int examId);
        bool HasStudentTakenTask(int studentId, int examId);



    }
}