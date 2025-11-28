using Luno_platform.Models;
using Luno_platform.Viewmodel;

namespace Luno_platform.Service
{
    public interface IParentService : I_BaseService<Parent>
    {
        public List<Student> GetStds(int id);
        public List<Classes> GetClasses();
        List<Courses> GetStudentCourses(int id);
        List<Courses> GetStudentCourses(int studentId, int page = 1, int pageSize = 10);
        public Student GetStudentDetails(int id);
        public Student FindChild(string nationalID);
        public void AddChild(ChildViewModel childvm);
        public bool LinkChild(int parentID, int studentID, out string error);
        public Parent GetParent(int id);
        public ParentSettingVM GetParentSetting(int ParentId);
        public bool UpdateParentSetting(ParentSettingVM pVM);

        public bool ChangeParentPassword(int parentId, string currentPassword, string newPassword);
        public void UpdateImage(int parentId, string imgUrl);
        List<Payments> GetPayments(int studentId);

    }
}
