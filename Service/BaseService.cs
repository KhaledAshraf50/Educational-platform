using Luno_platform.Models;
using Luno_platform.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public int? GetStudentIdByUserId(int userId)
        {
         return Repo.GetStudentIdByUserId(userId);
        }
        public int? GetParentIdByUserId(int userId) { 
         return Repo.GetParentIdByUserId(userId);
        }
        public int? GetInstructorIdByUserId(int userId)
        {
         return Repo.GetInstructorIdByUserId(userId);
        }

        public bool HasStudentTakenExam(int studentId, int examId)
        {
            return  Repo.HasStudentTakenExam(studentId,examId);
        }


        public bool HasStudentTakenTask(int studentId, int examId)
        {
            return Repo.HasStudentTakenTask(studentId, examId);
        }

    }

}