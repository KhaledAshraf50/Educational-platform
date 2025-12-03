using Luno_platform.Models;
using Luno_platform.Repository;
using Luno_platform.Viewmodel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Luno_platform.Service
{
    public class ParentService : BaseService<Parent>, IParentService
    {
        IParentRepo parentRepo;
        IstudentRepo studentRepo;
        public ParentService(IParentRepo repo, IstudentRepo studentRepo) : base(repo)
        {
            parentRepo = repo;
            this.studentRepo = studentRepo;
        }
        public List<Student> GetStds(int id)
        {
            return parentRepo.GetStds(id);
        }
        public List<Classes> GetClasses()
        {
            return parentRepo.GetClasses();
        }

        public Student GetStudentDetails(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid student ID");
            }
            return parentRepo.GetStudentDetails(id);
        }
        public void AddChild(ChildViewModel childvm)
        {
            if (childvm == null)
            {
                throw new ArgumentNullException(nameof(childvm), "Invalid National ID");
            }
            var student = parentRepo.GetStudentByNationalID(childvm.NationalID);
            if (student == null)
            {
                throw new ArgumentException("No Exist Student With This National ID");
            }
            student.ParentId = childvm.ParentID;
            parentRepo.UpdateStudent(student);
        }
        public Student FindChild(string nationalID)
        {
            return parentRepo.GetStudentByNationalID(nationalID);
        }
        public bool LinkChild(int parentID, int studentID, out string errorMessage)
        {
            errorMessage = null;
            var student = parentRepo.GetStudentById(studentID);
            if (student == null)
            {
                errorMessage = "No Exist Student With This ID";
                return false;
            }
            if (student.ParentId != null && student.ParentId != parentID)
            {
                errorMessage = "This Student Is Linked With Another Parent";
                return false;
            }
            student.ParentId = parentID;
            parentRepo.UpdateStudent(student);
            return true;
        }
        public Parent GetParent(int id)
        {
            return parentRepo.GetParent(id);
        }

        public ParentSettingVM GetParentSetting(int id)
        {
            var parent = parentRepo.GetParent(id);
            ParentSettingVM pVM = new ParentSettingVM()
            {
                ParentID = parent.ID,
                Image = parent.User.Image,
                Name = parent.User.fname + " " + parent.User.lastName,
                Email = parent.User.Email,
                PhoneNumber = parent.User.PhoneNumber,
                NoOfChildren = parentRepo.GetNoOfStudents(id)
            };
            return pVM;
        }

        public bool UpdateParentSetting(ParentSettingVM pVM)
        { 
           var parent = parentRepo.GetParent(pVM.ParentID);
            if (parent == null || parent.User == null)
                return false;
            parent.User.fname = pVM.Name.Split(' ')[0];
            parent.User.lastName = pVM.Name.Split(' ').Length > 1 ? pVM.Name.Split(' ')[1] : "";
            parent.User.Email = pVM.Email;
            parent.User.PhoneNumber = pVM.PhoneNumber;
            parent.User.nationalID = pVM.NationalId;
            parentRepo.Update(parent);
            parentRepo.Save();
            return true;
        }

        public bool ChangeParentPassword(int parentId, string oldPassword, string newPassword)
        {
            var parent = parentRepo.GetParent(parentId);
            if (parent == null) return false;
            var hasher = new PasswordHasher<Users>();
            var result = hasher.VerifyHashedPassword(parent.User, parent.User.PasswordHash, oldPassword);
            if (result == PasswordVerificationResult.Failed)
            {
                return false;
            }
            //parent.User.PasswordHash = newPassword;
            // تشفير الباسورد الجديد
            parent.User.PasswordHash = hasher.HashPassword(parent.User, newPassword);
            parentRepo.Update(parent);
            parentRepo.Save();
            return true;
        }

        public List<Courses> GetStudentCourses(int id)
        {
            return studentRepo.GetStudentCourses(id);
        }

        public List<Courses> GetStudentCourses(int studentId, int page = 1, int pageSize = 10)
        {
            return studentRepo.GetStudentCourses(studentId, page, pageSize);
        }

  
        public void UpdateImage(int parentId, string imgUrl)
        {
            var parent = parentRepo.GetParent(parentId);

            if (parent == null || parent.User == null)
                throw new Exception("Parent not found");

            parent.User.Image= imgUrl;

            parentRepo.Update(parent);
            parentRepo.Save();
        }
        public List<Payments> GetPayments(int studentId)
        {
            return parentRepo.GetPayments(studentId);
        }

        public List<showallStudent> showparent()
        {
            return parentRepo.showparent();
        }

        public void deleteparent(int userid)
        {
            parentRepo.deleteparent(userid);
        }
    }
}
