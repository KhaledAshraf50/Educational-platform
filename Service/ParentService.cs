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
                Image = parent.Image,
                Name = parent.User.fname+" "+ parent.User.lastName,
                Email = parent.User.email,
                PhoneNumber = parent.User.phoneNumber,
                NoOfChildren = parentRepo.GetNoOfStudents(ParentId)
                //NoOfChildren = parent.Students.Count()
            };
            return pVM;
        }

        public ParentSettingVM UpdateParentSetting(ParentSettingVM pVM)
        {
            throw new NotImplementedException();
        }

        public bool ChangeParentPassword(int parentId, string currentPassword, string newPassword)
        {
            var parent = parentRepo.GetParent(parentId);
            if (parent == null) return false;
            if (parent.User.password != currentPassword) return false;
            parent.User.password = newPassword;
            parentRepo.Update(parent);
            return true;
        }
        public void UpdateImage(int parentId, string imgUrl)
        {
            var parent = parentRepo.GetParent(parentId);

            if (parent == null || parent.User == null)
                throw new Exception("Parent not found");

            parent.Image = imgUrl;

            parentRepo.Update(parent);
        }
    }
}
