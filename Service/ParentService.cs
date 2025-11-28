using Luno_platform.Models;
using Luno_platform.Repository;
using Luno_platform.Viewmodel;

namespace Luno_platform.Service
{
    public class ParentService : BaseService<Parent>, IParentService
    {
        IParentRepo parentRepo;
        public ParentService(IParentRepo repo):base(repo)
        {
            parentRepo = repo;
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
            if(id <= 0)
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
        public bool LinkChild(int parentID, int studentID,out string errorMessage)
        {
            errorMessage = null;
            var student = parentRepo.GetStudentById(studentID);
            if (student == null)
            {
                errorMessage = "No Exist Student With This ID";
                return false;
            }
            if(student.ParentId!=null && student.ParentId != parentID)
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

        public ParentSettingVM GetParentSetting(int ParentId)
        {
           var parent = parentRepo.GetParent(ParentId);
            ParentSettingVM pVM = new ParentSettingVM()
            {
                ParentID = parent.ID,
                //Image = parent.Image,
                Name = parent.User.fname+" "+ parent.User.lastName,
                Email = parent.User.Email,
                PhoneNumber = parent.User.PhoneNumber,
                NoOfChildren = parentRepo.GetNoOfStudents(ParentId)
                //NoOfChildren = parent.Students.Count()
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
            if (parent.User.PasswordHash != oldPassword) return false;
            parent.User.PasswordHash = newPassword;
            parentRepo.Update(parent);
            parentRepo.Save();
            return true;
        }

        //public void UpdateImage(int parentId, string imgUrl)
        //{
        //    throw new NotImplementedException();
        //}
        public void UpdateImage(int parentId, string imgUrl)
        {
            var parent = parentRepo.GetParent(parentId);

            if (parent == null || parent.User == null)
                throw new Exception("Parent not found");

            parent.User.Image  = imgUrl;

            parentRepo.Update(parent);
            parentRepo.Save();
        }
    }
}
