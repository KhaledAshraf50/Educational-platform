using Luno_platform.Models;
using System.Collections.Generic;

namespace Luno_platform.Repository
{
    public interface I_BaseRepository<T> where T : class
    {

        List<T> GetAll();
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

        public List<Subject> subjects();


    }
}