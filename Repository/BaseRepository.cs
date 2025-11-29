using Luno_platform.Models;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Repository
{
    public class BaseRepository<T> : I_BaseRepository<T> where T : class
    {

        public LunoDBContext _Context;
        public DbSet<T> Table;

        public BaseRepository(LunoDBContext lunoDBContext)
        {
            _Context = lunoDBContext;
            Table = _Context.Set<T>();
        }
        public int? GetStudentIdByUserId(int userId)
        {
            var student = _Context.Students
                .Include(s => s.User)
                .FirstOrDefault(s => s.User.Id == userId);

            return student?.StudentID;   // لو مش طالب هترجع null
        }

        // ترجع ParentID بناءً على UserId
        public int? GetParentIdByUserId(int userId)
        {
            var parent = _Context.Parents
                .Include(p => p.User)
                .FirstOrDefault(p => p.User.Id == userId);

            return parent?.ID;   // لو مش موجود بيرجع null
        }

        // ترجع InstructorID بناءً على UserId
        public int? GetInstructorIdByUserId(int userId)
        {
            var instructor = _Context.Instructors
                .Include(i => i.User)
                .FirstOrDefault(i => i.User.Id == userId);

            return instructor?.instructorID;   // لو مش موجود بيرجع null
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
        public bool HasStudentTakenExam(int studentId, int examId)
        {
            return _Context.StudentStatistics.Any(s => s.StudentID == studentId && s.ExamId == examId);
        }


        public bool HasStudentTakenTask(int studentId, int examId)
        {
            return _Context.Studentstaistics_In_Tasks.Any(s => s.StudentID == studentId && s.TaskId == examId);
        }
    }
}