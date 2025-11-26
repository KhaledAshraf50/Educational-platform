using Luno_platform.Models;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Repository
{
    public class ParentRepo : BaseRepository<Parent>,IParentRepo
    {
        public ParentRepo(LunoDBContext db):base(db)
        {

        }

        public List<Student> GetStds(int id)
        {
            return _Context.Students.Include(u=>u.User).Include(u=>u.Classes).Where(p=>p.ParentId==id).ToList();
        }
        public List<Classes> GetClasses()
        {
            return _Context.Classes.ToList();
        }
        public Student GetStudentDetails(int id)
        {
            return _Context.Students.Include(u=>u.User).Include(c=>c.Classes).FirstOrDefault(s=>s.StudentID==id);
        }
        public Student GetStudentByNationalID(string nationalID)
        {
            return _Context.Students.Include(u=>u.User).Include(c=>c.Classes).FirstOrDefault(s=>s.User.nationalID==nationalID);
        }
       public Student GetStudentById(int id)
       {
            return _Context.Students.Find(id);
       }
        public void UpdateStudent(Student student)
        {
            _Context.Students.Update(student);
            _Context.SaveChanges();
        }
        public Parent GetParent(int id)
        {
            var parent = _Context.Parents.Include(u => u.User).FirstOrDefault(p => p.User.Id == id);
            if(parent == null)
            {
                throw new Exception("Parent not found");
            }
            return parent;
        }
        public Parent EditParentSetting(ParentSettingVM pVM)
        {
            var parent = _Context.Parents.Include(u => u.User).FirstOrDefault(p => p.ID == pVM.ParentID);
            if (parent == null)
            {
                throw new Exception("Parent not found");
            }
            return parent;
        }

        public int GetNoOfStudents(int parentId)
        {
            int count = _Context.Students.Count(s => s.ParentId == parentId);
            return count;
        }
    }
}
